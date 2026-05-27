using System.Text.Json;

namespace SpassuDesafio.API.MIddleware
{
    public class ExceptionHadnleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHadnleMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHadnleMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHadnleMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                var error = MappedException(e);

                _logger.Log(
                    error.LogLevel,
                    e,
                    "Falha na requisição {Method} {Path}. Status: {StatusCode}. TraceId: {TraceId}",
                    context.Request.Method,
                    context.Request.Path,
                    error.StatusCode,
                    context.TraceIdentifier);

                await HandleExceptionAsync(context, e, error);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception e, ExceptionMapped error)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = error.StatusCode;

            var body = new
            {
                title = error.Title,
                statusCode = error.StatusCode,
                detail = _env.IsDevelopment() ? e.ToString() : error.Title,
                instance = context.Request.Path.ToString()
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(body));
        }

        private static ExceptionMapped MappedException(Exception e) => e switch
        {
            KeyNotFoundException => new(StatusCodes.Status404NotFound, "Recurso não localizado.", LogLevel.Warning),
            InvalidOperationException => new(StatusCodes.Status400BadRequest, "Operação inválida.", LogLevel.Warning),
            ArgumentException => new(StatusCodes.Status400BadRequest, "Argumento inválido.", LogLevel.Warning),
            _ => new(StatusCodes.Status500InternalServerError, "Erro interno do servidor.", LogLevel.Error)
        };

        private record ExceptionMapped(int StatusCode, string Title, LogLevel LogLevel);
    }
}
