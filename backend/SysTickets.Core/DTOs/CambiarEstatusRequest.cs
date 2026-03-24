using System.ComponentModel.DataAnnotations;

namespace SysTickets.Core.DTOs;

public class CambiarEstatusRequest
{
    [Required]
    [RegularExpression("(?i)^(abierto|en[ _]?progreso|resuelto|cerrado)$", ErrorMessage = "Estatus no válido")]
    public string Estatus { get; set; } = string.Empty;
}
