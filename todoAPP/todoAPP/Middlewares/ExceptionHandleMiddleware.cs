using System;
namespace todoAPP.Middlewares
{
	public class ExceptionHandleMiddleware
	{
        private readonly RequestDelegate _next;

        public ExceptionHandleMiddleware(RequestDelegate next)
		{
            _next = next;
		}

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            return context.Response.WriteAsync($"{context.Response.StatusCode} Internal Server Error.");
        }
    }
}

