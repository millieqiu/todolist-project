using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using todoAPP.Middlewares;
using todoAPP.Models;
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
        builder.Services.AddRazorPages();

        builder.Services.AddControllers();

        builder.Services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("TodoDbConnection"));
        });

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(
            options =>
            {
                options.LoginPath = "/Index";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
            }
            );

        builder.Services.AddHttpClient();

        builder.Services.AddTransient<TodoListService>();
        builder.Services.AddTransient<AuthService>();
        builder.Services.AddTransient<UserService>();
        builder.Services.AddTransient<RoleService>();
        builder.Services.AddSingleton<WeatherService>();
        builder.Services.AddTransient<FileService>();
        builder.Services.AddTransient<OAuthService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseExceptionHandleMiddleware();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllers();
        });

        app.Run();
    }
}
