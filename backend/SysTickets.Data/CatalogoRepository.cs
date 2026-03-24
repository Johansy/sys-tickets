using Npgsql;
using SysTickets.Core.Interfaces;
using Dapper;
using SysTickets.Core.Models;


namespace SysTickets.Data;

public class CatalogoRepository: ICatalogoRepository
{
    private readonly string _connectionString;

    public CatalogoRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    private NpgsqlConnection CreateConnection() => new NpgsqlConnection(_connectionString);
    

    public async Task<IEnumerable<Categoria>> ObtenerCategoriasAsync()
    {

       using var conn = CreateConnection();
       return await conn.QueryAsync<Categoria>("SELECT * FROM categorias ORDER BY nombre");
    }

    public async Task<IEnumerable<Usuario>> ObtenerUsuariosAsync()
    {
        using var conn = CreateConnection();
        return await conn.QueryAsync<Usuario>("SELECT * FROM usuarios ORDER BY nombre");
    }

    public async Task<IEnumerable<Usuario>> ObtenerAgentesAsync()
    {
        using var conn = CreateConnection();
        return await conn.QueryAsync<Usuario>("SELECT * FROM usuarios WHERE rol = 'Agente' ORDER BY nombre");
    }


    public async Task<Usuario?> ObtenerUsuarioPorIdAsync(int id)
    {
        using var conn = CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Usuario>("SELECT * FROM usuarios WHERE id = @Id", new { Id = id });
    }
}
