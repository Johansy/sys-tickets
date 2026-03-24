using System.ComponentModel.DataAnnotations;

namespace SysTickets.Core.DTOs;

public class AgregarComentarioRequest
{
    [Required]
    public string Contenido { get; set; } = string.Empty;
    public bool EsInterno { get; set; }
}
