using EncryptionProject.Logic;
using EncryptionProject.Logic.Algorithms;
using EncryptionProject.Logic.Interfaces;
using EncryptionProject.Models;

namespace EncryptionProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddMvc();
            builder.Services.Configure<MySettings>(builder.Configuration.GetSection("MySettings"));
            builder.Services.AddSingleton<EncrypFactory>();
            builder.Services.AddSingleton<ICoding, CaesarLogic>();
            builder.Services.AddSingleton<ICoding, VigenereLogic>();
            builder.Services.AddSingleton<ICoding, AtbashLogic>();

            var app = builder.Build();

            app.UseStaticFiles();

            app.MapControllerRoute("default", "{controller=Home}/{action=Welcome}");

            app.Run();
        }
    }
}
