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

    var parameters = new DynamicParameters();
    parameters.Add("p_nombre", contacto.Nombre);
    parameters.Add("p_telefono", contacto.Telefono);
    parameters.Add("p_pais", contacto.Pais);
    parameters.Add("p_departamento", contacto.Departamento);
    parameters.Add("p_municipio", contacto.Municipio);
    parameters.Add("p_direccion", contacto.Direccion);
    parameters.Add("p_id_generado", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

    await connection.ExecuteAsync(
        "public.sp_registrar_contacto",
        parameters,
        commandType: CommandType.StoredProcedure
    );
  }
}