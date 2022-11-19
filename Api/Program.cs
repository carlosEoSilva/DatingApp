using Api.Data;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //-cria o host buscando informações nos arquivos de configração do projeto.Ex:appsettings.json
            var host= CreateHostBuilder(args).Build();
            using var scope= host.Services.CreateScope();
            var services= scope.ServiceProvider;
            try
            {
                var context= services.GetRequiredService<DataContext>();
                await context.Database.MigrateAsync();
                await Seed.SeedUsers(context);

            }catch(Exception ex)
            {
                var logger= services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occured during migration!");
            }

            await host.RunAsync();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
