using Almoxarifado.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Almoxarifado.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Funcionario> Funcionarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Funcionario>()
                .HasIndex(f => f.Cpf)
                .IsUnique();
        }
    }
}