using Microsoft.AspNetCore.Mvc;
using SysTickets.Core.Interfaces;
using SysTickets.Core.DTOs;

namespace SysTickets.API.Controllers;


[ApiController]
[Route("api/tickets")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _service;

    public TicketsController(ITicketService service)
    {
        _service = service;
    }

    // Helper: obtiene el ID del usuario simulado desde el header X-Usuario-Id
    private int GetUsuarioId()
    {
        if (Request.Headers.TryGetValue("X-Usuario-Id", out var value) && int.TryParse(value, out var id))
        
            return id;
            return 1; // Valor por defecto para pruebas
    }

   
    //GET /api/tickets?texto= ...&categoriaId=1&prioridad=alta&estatus=abierto&agenteId=6&pagina=1&registros=10
    [HttpGet]
    public async Task<IActionResult> Buscar([FromQuery] TicketFiltrosRequest filtros)
    {        
        var result = await _service.BuscarTicketsAsync(filtros);
        return Ok(result);
    }

    //GET /api/tickets/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerDetalle(int id)
    {
        try
        {
            var ticket = await _service.ObtenerDetalleAsync(id);
            if (ticket is null) return NotFound();
            return Ok(ticket);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }

    //POST /api/tickets
    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearTicketRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var usuarioId = GetUsuarioId();
        var id = await _service.CrearAsync(request, usuarioId);
        return CreatedAtAction(nameof(ObtenerDetalle), new { id }, new { id });
    }

    //PATCH /api/tickets/{id}/estatus
    [HttpPatch("{id:int}/estatus")]
    public async Task<IActionResult> CambiarEstatus(int id, [FromBody] CambiarEstatusRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var usuarioId = GetUsuarioId();
        try
        {   
            await _service.CambiarEstatusAsync(id, request, usuarioId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return UnprocessableEntity(new { mensaje = ex.Message });
        }
    }

    //PATCH /api/tickets/{id}/asignar
    [HttpPatch("{id:int}/asignar")]
    public async Task<IActionResult> AsignarAgente(int id, [FromBody] AsignarAgenteRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var usuarioId = GetUsuarioId();
        try
        {
            await _service.AsignarAgenteAsync(id, request, usuarioId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return UnprocessableEntity(new { mensaje = ex.Message });
        }
    }

    //POST /api/tickets/{id}/comentarios
    [HttpPost("{id:int}/comentarios")]
    public async Task<IActionResult> AgregarComentario(int id, [FromBody] AgregarComentarioRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var autorId = GetUsuarioId();
        try
        { 
            var comentarioId = await _service.AgregarComentarioAsync(id, request, autorId);
            return Ok(new { id = comentarioId });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return UnprocessableEntity(new { mensaje = ex.Message });
        }
    }
}