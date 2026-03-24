namespace SysTickets.Core.Models;

public class Comentario
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public int AutorId { get; set; }
    public string Contenido { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public bool EsInterno { get; set; }
}
