using SysTickets.Core.DTOs;

namespace SysTickets.Core.Interfaces;

public interface ITicketService
{
    Task<PaginatedResult<TicketListItemDto>> BuscarTicketsAsync(TicketFiltrosRequest filtros);
    Task<TicketDetalleDto?> ObtenerDetalleAsync(int id);
    Task<int> CrearAsync(CrearTicketRequest request, int usuarioId);
    Task CambiarEstatusAsync(int id, CambiarEstatusRequest request, int usuarioId);
    Task AsignarAgenteAsync(int id, AsignarAgenteRequest request, int usuarioId);
    Task<int> AgregarComentarioAsync(int ticketId, AgregarComentarioRequest request, int autorId);
}
