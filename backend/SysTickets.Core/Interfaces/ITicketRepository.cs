using SysTickets.Core.Models;
using SysTickets.Core.DTOs;

namespace SysTickets.Core.Interfaces;

public interface ITicketRepository
{
    Task<PaginatedResult<TicketListItemDto>> BuscarAsync(TicketFiltrosRequest filtros);
    Task<TicketDetalleDto?> ObtenerDetalleAsync(int id);
    Task<int> CrearAsync(Ticket ticket);
    Task<bool> CambiarEstatusAsync(int id, string estatus, DateTime? fechaResolucion);
    Task<bool> AsignarAgenteAsync(int id, int agenteId);
    Task<int> AgregarComentarioAsync(Comentario comentario);
    Task<Ticket?> ObtenerPorIdAsync(int id);
    Task RegistrarHistorialAsync(HistorialTicket historial);
}