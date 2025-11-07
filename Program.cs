using Microsoft.EntityFrameworkCore;
using MaquinariaApi.Data;
using MaquinariaApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configurar SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=maquinaria.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Crear base de datos si no existe
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// Habilitar Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => " API de Clientes funcionando. Visita /swagger");

// CRUD CLIENTES

app.MapGet("/clientes", async (AppDbContext db) =>
    await db.Clientes.ToListAsync());

app.MapGet("/clientes/{id:int}", async (int id, AppDbContext db) =>
    await db.Clientes.FindAsync(id) is Cliente cliente
        ? Results.Ok(cliente)
        : Results.NotFound());

app.MapPost("/clientes", async (Cliente cliente, AppDbContext db) =>
{
    db.Clientes.Add(cliente);
    await db.SaveChangesAsync();
    return Results.Created($"/clientes/{cliente.Id}", cliente);
});

app.MapPut("/clientes/{id:int}", async (int id, Cliente updated, AppDbContext db) =>
{
    var cliente = await db.Clientes.FindAsync(id);
    if (cliente is null) return Results.NotFound();

    cliente.NombreCompleto = updated.NombreCompleto;
    cliente.TipoIdentificacion = updated.TipoIdentificacion;
    cliente.Documento = updated.Documento;
    cliente.Telefono = updated.Telefono;
    cliente.Email = updated.Email;
    cliente.Departamento = updated.Departamento;
    cliente.Direccion = updated.Direccion;
    cliente.TipoOrganizacion = updated.TipoOrganizacion;

    await db.SaveChangesAsync();
    return Results.Ok(cliente);
});

app.MapDelete("/clientes/{id:int}", async (int id, AppDbContext db) =>
{
    var cliente = await db.Clientes.FindAsync(id);
    if (cliente is null) return Results.NotFound();

    db.Clientes.Remove(cliente);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
