using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderManagementCore;
using OrderManagementCore.Interfaces;
using OrderManagementCore.Services;
using OrderManagementStorage;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("local");

builder.Services.AddControllers();

builder.Services.AddDbContext<OrderContext>(opt => opt.UseSqlServer(connectionString));
builder.Services.AddScoped<IOrderService, OrderService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();