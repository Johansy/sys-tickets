using SysTickets.Core.Models;

namespace SysTickets.Core.Interfaces;

public interface ICatalogoRepository
{
    Task<IEnumerable<Categoria>> ObtenerCategoriasAsync();
    Task<IEnumerable<Usuario>> ObtenerUsuariosAsync();
    Task<IEnumerable<Usuario>> ObtenerAgentesAsync();
    Task<Usuario?> ObtenerUsuarioPorIdAsync(int id);
}
