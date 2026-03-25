using System.ComponentModel.DataAnnotations;

namespace SysTickets.Core.DTOs;

public class TicketFiltrosRequest
{
    public string? Texto { get; set; }

    public string? Titulo { get; set; }
    public int? CategoriaId { get; set; }
    public string? Prioridad { get; set; }
    public string? Estatus { get; set; }
    public int? AgenteId { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "La pagina debe ser mayor o igual a 1")]
    public int Pagina { get; set; } = 1;
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad de registros debe ser mayor o igual a 1")]
    public int Registros { get; set; } = 10;
}
