using Microsoft.AspNetCore.Mvc;
using SysTickets.Core.Interfaces;

namespace SysTickets.API.Controllers;

[ApiController]
[Route("api/catalogos")]
public class CatalogosController : ControllerBase
{
    private readonly ICatalogoRepository _repo;

    public CatalogosController(ICatalogoRepository repo)
    {
        _repo = repo;
    }

    // GET /api/catalogos/categorias
    // GET /api/catalogos/usuarios
    // GET /api/catalogos/agentes
    [HttpGet("{tipo}")]
    public async Task<IActionResult> Obtener(string tipo)
    {
        return tipo.ToLower() switch
        {
            "categorias" => Ok(await _repo.ObtenerCategoriasAsync()),
            "usuarios"   => Ok(await _repo.ObtenerUsuariosAsync()),
            "agentes"    => Ok(await _repo.ObtenerAgentesAsync()),
            _ => BadRequest(new { message = $"Tipo de catalogo '{tipo}' no reconocido." })
        };
    }
}
