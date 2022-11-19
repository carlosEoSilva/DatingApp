using Api.Extensions;
using Api.Middleware;

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
            //-método de extensão
            services.AddApplicationServices(_config);

            //-método de extensão
            services.AddIdentityServices(_config);

            services.AddControllers();

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //-dentro do 'Configure' a ordem importa.       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //-middleware criado para tratar erros
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            //-configurar o 'cors' para aceitar qualquer tipo de header, qualquer tipo de método(get,post,etc), da origem especificada com o 'WithOrigins'.
            app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
