using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PhotoPicker.Infrastructure.Database;

namespace PhotoPicker
{
    // TODO: Test user creation/login
    // TODO: Upload photos to S3
    // TODO: Face processing
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            // Controller base
            builder.Services.AddControllers();

            // Authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                            .AddCookie(options =>
                            {
                                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                                options.SlidingExpiration = true;
                            });

            // Authorization
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Authenticated", policy => policy.RequireAuthenticatedUser()) ;
                options.AddPolicy("PhotographerOnly", policy => policy.RequireRole("Photographer"));
                options.DefaultPolicy = options.GetPolicy("Authenticated")!;
            });

            // Add DB Context
            builder.Services.AddDbContextPool<DataContext>(
                options => options.UseMySQL("Server=localhost;Database=photopicker;Uid=root;Pwd=root")
            );

            var app = builder.Build();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.Run("http://0.0.0.0:80");
        }
    }
}