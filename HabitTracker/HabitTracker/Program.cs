using HabitTracker.Components;
using HabitTracker.DataBase;
using HabitTracker.DataBase.Repositories;
using HabitTracker.Models;
using HabitTracker.Services.Authentication;
using HabitTracker.Services.HabitLogics;
using HabitTracker.Services.JWT;
using HabitTracker.Services.Pages.LocalStorage;
using HabitTracker.Services.Pages.Requests;
using HabitTracker.Services.PasswordHashing;
using HabitTracker.Services.Profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HabitTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddBlazorBootstrap();

            builder.Services.AddControllers();

            builder.Services.AddHttpClient("api", client =>
            {
                var baseUrl = builder.Configuration["ApiBaseUrl"];

                if (string.IsNullOrWhiteSpace(baseUrl))
                    throw new InvalidOperationException("ApiBaseUrl is missing");

                client.BaseAddress = new Uri($"{baseUrl}/api/");
            });

            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration.GetValue<string>("JWTOptions:Issuer"),
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration.GetValue<string>("JWTOptions:Audience"),
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWTOptions:SecretKey")!))
                };
            });

            builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo("/app/keys"));

            builder.Services.AddAuthorization();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionString"));
            });

            builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWTOptions"));

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            builder.Services.AddScoped<IAppMappingProfile, AppMappingProfile>();

            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

            builder.Services.AddScoped<IJWTProvider, JWTProvider>();

            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddScoped<IHabitService, HabitService>();

            builder.Services.AddScoped<ISendRequest, SendRequest>();

            builder.Services.AddScoped<IAppLocalStorage, AppLocalStorage>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.MapStaticAssets();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();

                    Thread.Sleep(5000);

                    context.Database.Migrate();
                    Console.WriteLine("--> Миграции успешно применились!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Ошибка при применении миграций: {ex.Message}");
                }
            }

            app.Run();
        }
    }
}
