
using Api.Data;
using Api.Interfaces;
using Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ITokenService, TokenService>();

            //-nesse momento é estabelecida a conexão com o banco.
            services.AddDbContext<DataContext>(options => 
            {
                //-'connection string' foi definida no 'appsettings.Development.json'
                //-a 'connection string' possui o nome do arquivo de banco de dados.
                //-busca o valor de 'DefaultConnection' dentro do arquivo 'appsettings.Development.json'.
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}