using Dapper;
using Npgsql;
using Coink.Models;
using System.Data;

namespace Coink.Data;

public class ContactoRepository : IContactoRepository
{
  private readonly string _connectionString;

  public ContactoRepository(IConfiguration configuration)
  {
    _connectionString = configuration.GetConnectionString("DefaultConnection")
                        ?? throw new InvalidOperationException("Cadena de conexión no encontrada.");
  }

  public async Task RegistrarContactoAsync(ContactoRequest contacto)
  {
    using var connection = new NpgsqlConnection(_connectionString);

    await connection.ExecuteAsync(
        "sp_RegistrarContacto",
        contacto,
        commandType: CommandType.StoredProcedure
    );
  }
}