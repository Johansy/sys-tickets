namespace SysTickets.Core.DTOs;

public class ComentarioDto
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public string Contenido { get; set; } = string.Empty;
    public int AutorId { get; set; }
    public string AutorNombre { get; set; } = string.Empty;
    public DateTime Fecha{ get; set; }
    public bool EsInterno { get; set; }
}
