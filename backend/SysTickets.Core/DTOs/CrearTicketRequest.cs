using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SysTickets.Core.DTOs;


public class CrearTicketRequest
{
    [Required, MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;
    [Required]                  
    public string Descripcion { get; set; } = string.Empty;
    [Required]
    public int CategoriaId { get; set; }
    [Required]
    [RegularExpression("baja|media|alta|urgente", ErrorMessage = "Prioridad no válida")]
    public string Prioridad { get; set; } = string.Empty;
}