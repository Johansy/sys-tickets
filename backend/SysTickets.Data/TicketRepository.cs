using SysTickets.Core.Interfaces;
using SysTickets.Core.DTOs;
using Npgsql;
using Dapper;
using SysTickets.Core.Models;

namespace SysTickets.Data;

public class TicketRepository: ITicketRepository
{
    private readonly string _connectionString;

    public TicketRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    private NpgsqlConnection CreateConnection() => new NpgsqlConnection(_connectionString);

    public async Task<PaginatedResult<TicketListItemDto>> BuscarAsync(TicketFiltrosRequest filtros)
    {
        using var conn = CreateConnection();
        var textoBusqueda = filtros.Texto ?? filtros.Titulo;

        var rows = await conn.QueryAsync<dynamic>("SELECT * From buscar_tickets(@p_texto, @p_categoria_id, @p_prioridad, @p_estatus, @p_agente_id, @p_pagina, @p_registros)",
            new
            {
                p_texto = textoBusqueda,
                p_categoria_id = filtros.CategoriaId,
                p_prioridad = filtros.Prioridad,
                p_estatus = filtros.Estatus,
                p_agente_id = filtros.AgenteId,
                p_pagina = filtros.Pagina,
                p_registros = filtros.Registros
            });

            var list = rows.ToList();
            var total = list.Count > 0 ? (long)list[0].total_registros : 0;

            return new PaginatedResult<TicketListItemDto>
            {
                Items = list.Select(r => new TicketListItemDto
                {
                    Id = (int)r.ticket_id,
                    Titulo = (string)r.titulo,
                    CategoriaId = (int)r.categoria_id,
                    CategoriaNombre = (string)r.categoria_nombre,
                    Prioridad = (string)r.prioridad,
                    Estatus = (string)r.estatus,
                    AgenteNombre = r.agente_nombre as string,
                    CreadorNombre = (string)r.creador_nombre,
                    FechaCreacion = (DateTime)r.fecha_creacion,
                    TotalComentarios = (long)r.total_comentarios
                }),
                TotalRegistros = (int)total,
                Pagina = filtros.Pagina,
                RegistrosPorPagina = filtros.Registros
            };
    }

    public async Task<TicketDetalleDto?> ObtenerDetalleAsync(int id)
    {
        using var conn = CreateConnection();
        var sql = 
        @"SELECT t.*, c.nombre AS categoria_nombre,
        u_c.nombre AS creador_nombre, u_a.nombre AS agente_nombre
        FROM   tickets t
        JOIN   categorias c   ON c.id  = t.categoria_id
        JOIN   usuarios   u_c ON u_c.id = t.creado_por
        LEFT JOIN usuarios u_a ON u_a.id  = t.asignado_a
        WHERE  t.id = @id;
        SELECT com.*, u.nombre AS autor_nombre
        FROM   comentarios com
        JOIN   usuarios    u   ON u.id = com.autor_id
        WHERE  com.ticket_id = @id
        ORDER BY com.fecha;
        SELECT h.*, u.nombre AS modificado_por_nombre
        FROM   ticket_historial h
        LEFT JOIN usuarios u ON u.id = h.modificado_por
        WHERE  h.ticket_id = @id
        ORDER BY h.fecha_cambio;";

        using var multi = await conn.QueryMultipleAsync(sql, new { id });
        
        var ticket = await multi.ReadFirstOrDefaultAsync<dynamic>();
        if (ticket == null) return null;

        var comentarios = (await multi.ReadAsync<dynamic>()).ToList();
        var historial = (await multi.ReadAsync<dynamic>()).ToList();

        return new TicketDetalleDto
        {
            Id = (int)ticket.id,
            Titulo = (string)ticket.titulo,
            Descripcion = (string)ticket.descripcion,
            CategoriaId = (int)ticket.categoria_id,
            CategoriaNombre = (string)ticket.categoria_nombre,
            Prioridad = (string)ticket.prioridad,
            Estatus = (string)ticket.estatus,
            AgenteNombre = ticket.agente_nombre as string,
            AsignadoA = ticket.asignado_a as int?,
            CreadorId = (int)ticket.creado_por,
            CreadorNombre = (string)ticket.creador_nombre,
            FechaCreacion = (DateTime)ticket.fecha_creacion,
            FechaResolucion = ticket.fecha_resolucion as DateTime?,
            Comentarios = comentarios.Select(c => new ComentarioDto
            {
                Id = (int)c.id,
                TicketId = (int)c.ticket_id,
                AutorId = (int)c.autor_id,  
                AutorNombre = (string)c.autor_nombre,
                Contenido = (string)c.contenido,
                EsInterno = (bool)c.es_interno,
                Fecha = (DateTime)c.fecha
            }),
            Historial = historial.Select(h => new HistorialDto
            {
                Id = (int)h.id,
                CampoModificado = (string)h.campo_modificado,   
                ValorAnterior = h.valor_anterior as string,
                ValorNuevo = h.valor_nuevo as string,
                ModificadoPorNombre = h.modificado_por_nombre as string,
                FechaModificacion = (DateTime)h.fecha_cambio
            })
        };
    }

    public async Task<int> CrearAsync(Ticket ticket)
    {
        using var conn = CreateConnection();
        return await conn.ExecuteScalarAsync<int>(
            @"INSERT INTO tickets (titulo, descripcion, categoria_id, prioridad, estatus, creado_por)
              VALUES (@Titulo, @Descripcion, @CategoriaId, @Prioridad, 'abierto', @CreadoPor)
              RETURNING id;",
            ticket);
    }

    public async Task<bool> CambiarEstatusAsync(int id, string estatus, DateTime? fechaResolucion)
    {
        using var conn = CreateConnection();
        var result = await conn.ExecuteAsync(
            @"UPDATE tickets SET estatus = @Estatus, fecha_resolucion = @FechaResolucion
              WHERE id = @Id;",
            new { Id = id, Estatus = estatus, FechaResolucion = fechaResolucion });
        return result > 0;
    }

    public async Task CambiarEstatusConHistorialAsync(int id, string estatus, DateTime? fechaResolucion, HistorialTicket historial)
    {
        using var conn = CreateConnection();
        await conn.OpenAsync();
        await using var transaction = await conn.BeginTransactionAsync();

        try
        {
            await conn.ExecuteAsync(
                @"UPDATE tickets SET estatus = @Estatus, fecha_resolucion = @FechaResolucion
                  WHERE id = @Id;",
                new { Id = id, Estatus = estatus, FechaResolucion = fechaResolucion },
                transaction);

            await conn.ExecuteAsync(
                @"INSERT INTO ticket_historial (ticket_id, campo_modificado, valor_anterior, valor_nuevo, modificado_por)
                  VALUES (@TicketId, @CampoModificado, @ValorAnterior, @ValorNuevo, @ModificadoPor);",
                historial,
                transaction);

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> AsignarAgenteAsync(int id, int agenteId)
    {
        using var conn = CreateConnection();
        var result = await conn.ExecuteAsync(
            @"UPDATE tickets SET asignado_a = @AgenteId
              WHERE id = @Id;",
            new {id, agenteId});
        return result > 0;
    }

    public async Task AsignarAgenteConHistorialAsync(int id, int agenteId, HistorialTicket historial)
    {
        using var conn = CreateConnection();
        await conn.OpenAsync();
        await using var transaction = await conn.BeginTransactionAsync();

        try
        {
            await conn.ExecuteAsync(
                @"UPDATE tickets SET asignado_a = @AgenteId
                  WHERE id = @Id;",
                new { Id = id, AgenteId = agenteId },
                transaction);

            await conn.ExecuteAsync(
                @"INSERT INTO ticket_historial (ticket_id, campo_modificado, valor_anterior, valor_nuevo, modificado_por)
                  VALUES (@TicketId, @CampoModificado, @ValorAnterior, @ValorNuevo, @ModificadoPor);",
                historial,
                transaction);

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<int> AgregarComentarioAsync(Comentario comentario)
    {
        using var conn = CreateConnection();
        return await conn.ExecuteScalarAsync<int>(
            @"INSERT INTO comentarios (ticket_id, contenido, autor_id, es_interno)
              VALUES (@TicketId, @Contenido, @AutorId, @EsInterno)
              RETURNING id;",
            comentario);
    }

    public async Task<Ticket?> ObtenerPorIdAsync(int id)
    {
        using var conn = CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Ticket>(
            @"SELECT * FROM tickets WHERE id = @id;",
            new { id });
    }

    public async Task RegistrarHistorialAsync(HistorialTicket historial)
    {
        using var conn = CreateConnection();
        await conn.ExecuteAsync(
            @"INSERT INTO ticket_historial (ticket_id, campo_modificado, valor_anterior, valor_nuevo, modificado_por)
              VALUES (@TicketId, @CampoModificado, @ValorAnterior, @ValorNuevo, @ModificadoPor);",
            historial);
    }
}
