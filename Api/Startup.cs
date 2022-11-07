using Api.Data;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Startup
    {

        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //-nesse momento é estabelecida a conexão com o banco.
            services.AddDbContext<DataContext>(options => 
            {
                //-'connection string' foi definida no 'appsettings.Development.json'
                //-a 'connection string' possui o nome do arquivo de banco de dados.
                //-busca o valor de 'DefaultConnection' dentro do arquivo 'appsettings.Development.json'.
                options.UseSqlite(_config.GetConnectionString("DefaultConnection"));
            });

            services.AddControllers();

            services.AddCors();
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //-dentro do 'Configure' a ordem importa.       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //-configurar o 'cors' para aceitar qualquer tipo de header, qualquer tipo de método(get,post,etc), da origem especificada com o 'WithOrigins'.
            app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
