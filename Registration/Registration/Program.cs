using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Extensions.Options;
using Registration.JWTLogic;
using Registration.Models;

namespace Registration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddMvc();
            builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JwtOptions"));
            builder.Services.AddScoped<JWTProvider>();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddApiAuthentication(builder.Configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseStaticFiles(); //Стили из wwwroot

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always
            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute("default", "{controller=Home}/{action=HomePage}");

            app.Run();
        }
    }
}
