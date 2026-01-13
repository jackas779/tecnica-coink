using System.ComponentModel.DataAnnotations;

namespace Coink.Models;

public class ContactoRequest
{
  [Required(ErrorMessage = "El nombre es obligatorio.")]
  [StringLength(100, MinimumLength = 3)]
  public string Nombre { get; set; } = string.Empty;

  [Required(ErrorMessage = "El teléfono es obligatorio.")]
  [Phone(ErrorMessage = "El formato del teléfono no es válido.")]
  public string Telefono { get; set; } = string.Empty;

  [Required(ErrorMessage = "El país es obligatorio.")]
  public string Pais { get; set; } = string.Empty;

  [Required(ErrorMessage = "El departamento es obligatorio.")]
  public string Departamento { get; set; } = string.Empty;

  [Required(ErrorMessage = "El municipio es obligatorio.")]
  public string Municipio { get; set; } = string.Empty;

  [Required(ErrorMessage = "La dirección es obligatoria.")]
  public string Direccion { get; set; } = string.Empty;
}