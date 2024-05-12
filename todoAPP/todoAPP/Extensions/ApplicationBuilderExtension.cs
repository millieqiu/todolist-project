using todoAPP.Middlewares;
namespace todoAPP.Extensions;

public static class ApplicationBuilderExtension
{
    public static IApplicationBuilder UseExceptionHandleMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandleMiddleware>();
    }
}

