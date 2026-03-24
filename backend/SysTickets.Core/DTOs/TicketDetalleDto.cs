namespace SysTickets.Core.DTOs;

public class TicketDetalleDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int CategoriaId { get; set; }
    public string CategoriaNombre { get; set; } = string.Empty;
    public string Prioridad { get; set; } = string.Empty;
    public string Estatus { get; set; } = string.Empty;
    public string? AgenteNombre { get; set; }
    public int? AsignadoA { get; set; }
    public int CreadorId { get; set; }
    public string CreadorNombre { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaResolucion { get; set; }
    public IEnumerable<ComentarioDto> Comentarios { get; set; } = Enumerable.Empty<ComentarioDto>();
    public IEnumerable<HistorialDto> Historial { get; set; } = Enumerable.Empty<HistorialDto>();
}
