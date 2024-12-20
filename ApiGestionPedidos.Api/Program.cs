using ApiGestionPedidos.Aplicacion.Repositorios;
using ApiGestionPedidos.Aplicacion.Servicios;
using ApiGestionPedidos.Infraestructura.Data;
using ApiGestionPedidos.Infraestructura.Repositorios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AplicacionDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    )
);

//Registrar los repositorios concretos de infraestructura 
builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
builder.Services.AddScoped<IProductoRepositorio, ProductoRepositorio>();
builder.Services.AddScoped<IPedidoRepositorio, PedidoRepositorio>();

//Registrar los servicios de aplicación
builder.Services.AddScoped<ClienteServicios>();
builder.Services.AddScoped<ProductoServicios>();
builder.Services.AddScoped<PedidoServicio>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
