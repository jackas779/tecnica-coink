using Coink.Models;

namespace Coink.Data;

public interface IContactoRepository
{
  Task RegistrarContactoAsync(ContactoRequest contacto);
}