using Coink.Data;
using Coink.Models;
using System.ComponentModel.DataAnnotations;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyeccion de Dependencias (Patron Repository)
builder.Services.AddScoped<IContactoRepository, ContactoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/api/contactos", async (ContactoRequest request, IContactoRepository repository) =>
{
    // Validacion manual del modelo (Requerimiento B)
    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(request);

    if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
    {
        return Results.BadRequest(validationResults);
    }

    // Consumo del repositorio (Requerimiento C y D)
    try
    {
        await repository.RegistrarContactoAsync(request);
        return Results.Ok(new { Mensaje = "Contacto registrado exitosamente" });
    }
    catch (PostgresException ex) when (ex.SqlState == "P0001")
    {
        // Capturamos el error específico del Stored Procedure
        return Results.Conflict(new { Mensaje = ex.Message });
    }
})
.WithName("RegistrarContacto")
.WithOpenApi();

app.Run();
