using Database.Context;
using Database.Repositories;
using DTO.Profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApi.Services;
using WebApi.Services.Interfaces;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddCors(options => {
                options.AddPolicy("localCors", policy =>
                {
                    policy.WithOrigins("https://localhost:7242", "http://localhost:5078")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWTSecret")))
                };
            });

            builder.Services.AddDbContext<ApplicationDBContext>();

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            builder.Services.AddScoped<UserRepository>();

            builder.Services.AddScoped<IAppMappingProfile, AppMappingProfile>();

            builder.Services.AddScoped<ITokenControl, TokenControl>();

            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

            builder.Services.AddScoped<ICartService, CartService>();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseCors("localCors");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.UseStaticFiles();

            app.Run();
        }
    }
}
