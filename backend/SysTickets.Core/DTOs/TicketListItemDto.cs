namespace SysTickets.Core.DTOs;

public class TicketListItemDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int CategoriaId { get; set; }
    public string CategoriaNombre { get; set; } = string.Empty;
    public string Prioridad { get; set; } = string.Empty;
    public string Estatus { get; set; } = string.Empty;
    public string? AgenteNombre { get; set; }
    public string CreadorNombre { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public long TotalComentarios { get; set; }
}
