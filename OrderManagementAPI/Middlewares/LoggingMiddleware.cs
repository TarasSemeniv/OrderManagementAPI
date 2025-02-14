using System.Text;
using OrderManagementAPI.Mapping;

namespace OrderManagementAPI.Middlewares;

public class LoggingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<LoggingMiddleware>>();
        
        context.Request.EnableBuffering();
        
        var result = await context.Request.BodyReader.ReadAsync();
        string body = Encoding.UTF8.GetString(result.Buffer);
        logger.LogInformation($"{DateTime.UtcNow.ToShortTimeString()} | [{context.Request.Method}] {context.Request.Path} : {Environment.NewLine}{body}");
        
        context.Request.Body.Position = 0;
        
        await next(context);

        logger.LogInformation($"{DateTime.UtcNow.ToShortTimeString()} | [{context.Response.StatusCode}] {context.Request.Path}");
    }
} 