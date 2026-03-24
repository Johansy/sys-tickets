using System.ComponentModel.DataAnnotations;

namespace SysTickets.Core.DTOs;

public class AsignarAgenteRequest
{
    [Required]
    public int AgenteId { get; set; }
}
