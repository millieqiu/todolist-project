using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using todoAPP.Extensions;
using todoAPP.Middlewares;
using todoAPP.Models;
using todoAPP.Models.ConfigModels;
using todoAPP.Services;

namespace todoAPP;

public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        //logging
        builder.Logging.ClearProviders();

        builder.Host.UseNLog();

        // Add services to the container.
        builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

        builder.Services.AddControllers();

        builder.Services.AddDbContext<DBContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
        });

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "TODOLIST";
                options.LoginPath = "/Login";
                options.SlidingExpiration = true;

                // 當要存取內容但驗證沒過時，會RedirectToLogin
                options.Events.OnRedirectToLogin = (context) =>
                {
                    if (context.Request.Path.StartsWithSegments("/api"))
                    {
                        context.Response.Clear();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    }
                    else
                    {
                        context.Response.Clear();
                        context.Response.Redirect("/Login");
                        return Task.CompletedTask;
                    }
                };
            });

        builder.Services.AddHttpClient();
        builder.Services.AddHttpContextAccessor();

        builder.Services.Configure<OAuthConfig>(builder.Configuration.GetSection(OAuthConfig.SectionName));
        builder.Services.AddTransient<ITodoService, TodoService>();
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<AvatarService>();
        builder.Services.AddTransient<IOAuthService, OAuthService>();
        builder.Services.AddTransient<IKanbanService, KanbanService>();
        builder.Services.AddTransient<IUserTagService, UserTagService>();

        builder.Services.AddDocumentService();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseMiddleware<ExceptionHandleMiddleware>();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();
        app.MapControllers();

        app.UseOpenApi();
        app.UseSwaggerUi3();

        app.Run();
    }
}
