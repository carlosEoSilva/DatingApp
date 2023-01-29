using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    //-esta classe é responsável por acessar o banco, outras classes que forem acessar o banco precisam de um 'DbContext'.
    //-preciso incluir o serviço 'DataContext' no 'Startup.cs'.
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options){ }

        //-9
        //-10
        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserLike> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //-11
            base.OnModelCreating(builder);

            builder.Entity<UserLike>()
                .HasKey(k => new{k.SourceUserId, k.TargetUserId});

            builder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserLike>()
                .HasOne(s => s.TargetUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(s => s.TargetUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}