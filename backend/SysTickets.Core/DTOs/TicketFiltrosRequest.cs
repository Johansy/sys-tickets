namespace SysTickets.Core.DTOs;

public class TicketFiltrosRequest
{
    public string? Texto { get; set; }

    public string? Titulo { get; set; }
    public int? CategoriaId { get; set; }
    public string? Prioridad { get; set; }
    public string? Estatus { get; set; }
    public int? AgenteId { get; set; }
    public int Pagina { get; set; } = 1;
    public int Registros { get; set; } = 10;
}
