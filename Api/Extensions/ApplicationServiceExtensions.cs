using Api.Data;
using Api.Helpers;
using Api.Interfaces;
using Api.Services;
using Api.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            //-5
            
            services.AddCors();

            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IPhotoService, PhotoService>();

            //-27
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSignalR();

            //-Primeira abordagem
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //-o 'config.Getsection' busca a configuração do campo 'CloudinarySettings' no arquivo 'appsettings.json'.
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

            //-nesse momento é estabelecida a conexão com o banco.
            services.AddDbContext<DataContext>(options => 
            {
                //-'connection string' foi definida no 'appsettings.Development.json'
                //-a 'connection string' possui o nome do arquivo de banco de dados.
                //-busca o valor de 'DefaultConnection' dentro do arquivo 'appsettings.Development.json'.
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            //-registrar atividades do usuário
            services.AddScoped<LogUserActivity>();

            //-24
            services.AddSingleton<PresenceTracker>();

            return services;
        }
    }
}