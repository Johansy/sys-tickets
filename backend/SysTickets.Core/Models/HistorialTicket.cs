namespace SysTickets.Core.Models;

public class HistorialTicket
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public string CampoModificado { get; set; } = string.Empty;
    public string? ValorAnterior { get; set; }
    public string? ValorNuevo { get; set; }
    public int? ModificadoPor { get; set; }
    public DateTime FechaModificacion { get; set; }
}
