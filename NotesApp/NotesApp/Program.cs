using Microsoft.Extensions.Options;
using NotesApp.DBLogic;
using NotesApp.Extensions;
using NotesApp.JwtLogic;
using NotesApp.Models;
using Microsoft.EntityFrameworkCore;

namespace NotesApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddMvc();
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
            builder.Services.Configure<EncryptOptions>(builder.Configuration.GetSection("EncryptOptions"));

            var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

            var connectionString = $"Server={dbHost};Database={dbName};User Id={dbUser};Password={dbPassword};TrustServerCertificate=True;";

            builder.Services.AddDbContext<DBConnection>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped<JwtProvider>();
            builder.Services.AddScoped<CryptoPassword>();
            builder.Services.AddScoped<CryptoText>();
            builder.Services.AddApiAuthentication(builder.Configuration);

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DBConnection>();
                db.Database.Migrate();
            }

            app.UseRouting();

            app.UseStaticFiles();

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always
            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute("default", "/{controller=Welcome}/{action=Registration}");

            app.MapControllerRoute("GetNote", "/{controller=Notes}/{action=Note}/{id}");

            app.Run();
        }
    }
}
