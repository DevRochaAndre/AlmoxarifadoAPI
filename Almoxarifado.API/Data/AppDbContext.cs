using Almoxarifado.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Almoxarifado.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Item> Itens { get; set; }

        // Novas tabelas da FASE 4:
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<EntradaNotaFiscal> EntradasNotasFiscais { get; set; }
        public DbSet<ItemEntradaNotaFiscal> ItensEntradasNotasFiscais { get; set; }

        // Novas tabelas da FASE 5:
        public DbSet<Requisicao> Requisicoes { get; set; }
        public DbSet<ItemRequisicao> ItensRequisicoes { get; set; }

        public DbSet<Devolucao> Devolucoes { get; set; }

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

            // Mapeamento do Preço Unitário no ItemEntradaNotaFiscal
            modelBuilder.Entity<ItemEntradaNotaFiscal>()
                .Property(p => p.PrecoUnitario)
                .HasPrecision(18, 2);

            // Mapeamento do Valor Total no EntradaNotaFiscal
            modelBuilder.Entity<EntradaNotaFiscal>()
                .Property(p => p.ValorTotalNota)
                .HasPrecision(18, 2);



        }
    }
}