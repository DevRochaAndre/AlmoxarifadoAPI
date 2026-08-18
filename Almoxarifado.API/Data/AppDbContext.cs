using Almoxarifado.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Almoxarifado.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Item> Itens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Garante que o CPF seja único
            modelBuilder.Entity<Funcionario>()
                .HasIndex(f => f.Cpf)
                .IsUnique();

            // Garante que a Matrícula seja única no banco
            modelBuilder.Entity<Funcionario>()
                .HasIndex(f => f.Matricula)
                .IsUnique();

            // Define o Codigo do Item como índice único no MySQL
            modelBuilder.Entity<Item>()
                .HasIndex(i => i.Codigo)
                .IsUnique();
        }
    }
}