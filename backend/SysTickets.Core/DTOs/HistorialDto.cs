namespace SysTickets.Core.DTOs;

public class HistorialDto
{
    public int Id { get; set; }
    public string CampoModificado { get; set; } = string.Empty;
    public string? ValorAnterior { get; set; }
    public string? ValorNuevo { get; set; }
    public string? ModificadoPorNombre { get; set; }
    public DateTime FechaModificacion { get; set; }
}
