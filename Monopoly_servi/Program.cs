using Microsoft.EntityFrameworkCore;
using Monopoly_servi.dao;
using Monopoly_servi.Hubs;
using Monopoly_servi.interfaz;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar el servicio de SignalR
builder.Services.AddSignalR();

// Add services to the container.
builder.Services.AddScoped<IJugadorInterfaz, JugadorImplDao>();

builder.Services.AddControllers();
// Learn more about configuring Swagger at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext with SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var app = builder.Build();

// 2. Mapear el Hub (la ruta que usará Flutter para conectarse)
app.MapHub<GameHub>("/gamehub");

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
