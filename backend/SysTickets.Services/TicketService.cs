using SysTickets.Core.Interfaces;
using SysTickets.Core.DTOs;
using SysTickets.Core.Models;

namespace SysTickets.Services;

public class TicketService: ITicketService
{
    private readonly ITicketRepository _ticketRepo;
    private readonly ICatalogoRepository _catalogoRepo;

    private static string NormalizarEstatus(string estatus)
        => estatus.Trim().ToLowerInvariant().Replace(" ", "_");

    private static bool EsRolAgente(string? rol)
        => string.Equals(rol?.Trim(), "Agente", StringComparison.OrdinalIgnoreCase);

    public TicketService(ITicketRepository ticketRepo, ICatalogoRepository catalogoRepo)
    {
        _ticketRepo = ticketRepo;
        _catalogoRepo = catalogoRepo;
    }

    public Task<PaginatedResult<TicketListItemDto>> BuscarTicketsAsync(TicketFiltrosRequest filtros) 
        => _ticketRepo.BuscarAsync(filtros);

    public async Task<TicketDetalleDto?> ObtenerDetalleAsync(int id)
    {
        var ticket = await _ticketRepo.ObtenerDetalleAsync(id)?? 
            throw new KeyNotFoundException($"Ticket {id} no encontrado.");
        return ticket;
    }

    public async Task<int> CrearAsync(CrearTicketRequest request, int usuarioId)
    {
        var ticket = new Ticket
        {
            Titulo = request.Titulo,
            Descripcion = request.Descripcion,
            CategoriaId = request.CategoriaId,
            Prioridad = request.Prioridad,
            CreadoPor = usuarioId
        };
        return await _ticketRepo.CrearAsync(ticket);
    }


    // REGLA 1 - No puede seguir el progreso sin un agente asignado.
    // REGLA (Extra) - Registra en el historial al cambiar el estatus.

    public async Task CambiarEstatusAsync(int id, CambiarEstatusRequest request, int usuarioId)
    {
        var ticket = await _ticketRepo.ObtenerPorIdAsync(id) ?? 
                throw new KeyNotFoundException($"Ticket {id} no encontrado.");

        var estatusNormalizado = NormalizarEstatus(request.Estatus);


        if (estatusNormalizado == "en_progreso" && ticket.AsignadoA == null)
            throw new InvalidOperationException(
                "El Ticket no puede pasar a 'En Progreso' sin un agente asignado.");

        DateTime? fechaResolucion = estatusNormalizado is "resuelto" or "cerrado" ? 
            DateTime.UtcNow : ticket.FechaResolucion;

        await _ticketRepo.CambiarEstatusConHistorialAsync(id, estatusNormalizado, fechaResolucion, new HistorialTicket
        {
            TicketId = id,
            CampoModificado = "estatus",
            ValorAnterior = ticket.Estatus,
            ValorNuevo = estatusNormalizado,
            ModificadoPor = usuarioId
        });
    }

    // REGLA 3 - Solo se puede asignar un usuario con rol de agente.
    // BONUS (Extra) - Registra en el historial al reasignar un agente
    public async Task AsignarAgenteAsync(int id, AsignarAgenteRequest request, int usuarioId)
    {
        var ticket = await _ticketRepo.ObtenerPorIdAsync(id) ??
            throw new KeyNotFoundException($"Ticket {id} no encontrado.");

        Usuario? agenteAnterior = null;
        if (ticket.AsignadoA.HasValue && ticket.AsignadoA.Value > 0)
            agenteAnterior = await _catalogoRepo.ObtenerUsuarioPorIdAsync(ticket.AsignadoA.Value);

        var agente = await _catalogoRepo.ObtenerUsuarioPorIdAsync(request.AgenteId) ?? 
            throw new KeyNotFoundException($"Usuario {request.AgenteId} no encontrado.");

        if (!EsRolAgente(agente.Rol))
            throw new InvalidOperationException($"El usuario {agente.Nombre} no tiene rol de agente.");

        await _ticketRepo.AsignarAgenteConHistorialAsync(id, request.AgenteId, new HistorialTicket
        {
            TicketId = id,
            CampoModificado = "asignado_a",
            ValorAnterior = agenteAnterior?.Nombre ?? "Sin asignar",
            ValorNuevo = agente.Nombre,
            ModificadoPor = usuarioId
        });
    }

    //REGLA 2 - Un ticket cerrado no acepta nuevos comentarios.
    public async Task<int> AgregarComentarioAsync(int ticketId, AgregarComentarioRequest request, int autorId)
    {
        var ticket = await _ticketRepo.ObtenerPorIdAsync(ticketId) ??
            throw new KeyNotFoundException($"Ticket {ticketId} no encontrado.");

        if (NormalizarEstatus(ticket.Estatus) == "cerrado")
            throw new InvalidOperationException(
                "No se pueden agregar comentarios a un ticket cerrado. Cambie el estatus primeramente.");

        return await _ticketRepo.AgregarComentarioAsync(new Comentario
        {
            TicketId = ticketId,
            Contenido = request.Contenido,
            AutorId = autorId,
            EsInterno = request.EsInterno
        });
    }
}