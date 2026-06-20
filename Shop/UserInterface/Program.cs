using UserInterface.Components;
using UserInterface.Services;
using UserInterface.Storage;

namespace UserInterface
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddHttpClient("api", conf =>
            {
                conf.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiSettings:BaseUrl")!);
            });

            builder.Services.AddBlazorBootstrap();

            builder.Services.AddScoped<SendRequestService>();

            builder.Services.AddSingleton<ImageService>();

            builder.Services.AddScoped<RoleService>();

            builder.Services.AddScoped<ProductInfoService>();

            builder.Services.AddScoped<ForceExitService>();

            builder.Services.AddScoped<ILocalStorage, LocalStorage>();

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

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
