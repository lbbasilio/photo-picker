using Microsoft.AspNetCore.Authentication.Cookies;

namespace PhotoPicker
{
    // TODO: authentication & authorization
    // TODO: DB layer, with EF Core
    // TODO: create user & photo objects
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, world!");
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

            var app = builder.Build();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.Run("http://0.0.0.0:80");

            /*Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://0.0.0.0:80");
                    webBuilder.UseKestrel();
                });*/

        }
    }
}