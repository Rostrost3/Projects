namespace BookManage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddMvc();

            var app = builder.Build();
            
            if(app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.MapControllerRoute("Default", "/{controller=Books}/{action=Index}");
            app.MapControllerRoute("GetById", "/{controller=Books}/{action=Details}/{id}");

            app.Run();
        }
    }
}
