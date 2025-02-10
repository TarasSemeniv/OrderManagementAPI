using AutoMapper;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Filters;
using OrderManagementAPI.Middlewares;
using OrderManagementCore;
using OrderManagementCore.Interfaces;
using OrderManagementCore.Services;
using OrderManagementStorage;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("local");

builder.Services.AddControllers(opt => opt.Filters.Add(new ExeptionFilter()));

builder.Services.AddDbContext<OrderContext>(opt => opt.UseSqlServer(connectionString));

builder.Services.AddTransient<LoggingMiddleware>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);
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

app.UseMiddleware<LoggingMiddleware>();


app.MapControllers();

app.Run();