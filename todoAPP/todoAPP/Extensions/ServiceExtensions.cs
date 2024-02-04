using System;
using NSwag;

namespace todoAPP.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDocumentService(this IServiceCollection collection)
        {
            return collection.AddOpenApiDocument(options =>
            {
                options.PostProcess = document =>
                {
                    document.Info = new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "TodoList",
                        Description = "An ASP.NET Core Web API for managing ToDo items"
                    };
                };
            });
        }

        public static IServiceCollection AddSingletonConfig<TConfig>(this IServiceCollection services, IConfiguration section) where TConfig : class
        {
            return services.AddSingleton(p =>
            {
                var instance = Activator.CreateInstance<TConfig>();
                section.Bind(instance);
                return instance;
            });
        }
    }
}

