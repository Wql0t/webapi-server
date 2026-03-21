using System.Net;
using System.Text.Json;

namespace pr.Middleware;

public class ExMiddleware {
    private readonly RequestDelegate _next;
    readonly ILogger<ExMiddleware> _logger;
    public ExMiddleware(RequestDelegate next, ILogger<ExMiddleware> logger) {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context) {
        try
        {
            await _next(context);
        }
        catch (Exception ex) {
            Console.WriteLine("==========================================================");
            Console.WriteLine("Error log");
            Console.WriteLine($"Time: {DateTime.Now}");
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine("==========================================================");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            Console.WriteLine("==========================================================");

            await HandleExceptionAsync(context);

        }
    }
    private static Task HandleExceptionAsync(HttpContext context){
        context.Response.ContentType = "appication/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var result = System.Text.Json.JsonSerializer.Serialize(new {
            error = "На сервере произошло внутренняя ошибка.",
            status = 500
        });

        return context.Response.WriteAsync(result);
    }
}