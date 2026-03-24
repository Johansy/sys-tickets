namespace SysTickets.Core.Models;

public class Ticket
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int CategoriaId { get; set; }
    public int CreadoPor { get; set; }
    public int? AsignadoA { get; set; }
    public string Estatus { get; set; } = string.Empty; // 'Abierto', 'En Proceso', 'Cerrado'
    public string Prioridad { get; set; } = string.Empty; // 'Baja', 'Media', 'Alta'
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaResolucion { get; set; }
}
