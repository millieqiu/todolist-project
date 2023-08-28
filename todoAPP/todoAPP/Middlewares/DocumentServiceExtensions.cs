using System;
using NSwag;

namespace todoAPP.Middlewares
{
	public static class DocumentServiceExtensions
	{
		public static IServiceCollection AddDocumentService(this IServiceCollection collection)
		{
            return collection.AddOpenApiDocument(options => {
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
    }
}

