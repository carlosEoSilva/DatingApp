using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    //-esta classe é responsável por acessar o banco, outras classes que forem acessar o banco precisam de um 'DbContext'.
    //-preciso incluir o serviço 'DataContext' no 'Startup.cs'.
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options){ }

        //-criar no banco uma tabela com nome de 'Users'.
        public DbSet<AppUser> Users { get; set; }

    }
}