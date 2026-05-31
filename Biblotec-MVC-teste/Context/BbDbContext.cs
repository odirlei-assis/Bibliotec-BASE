
using Bibliotec_MVC_teste.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblotec_MVC_teste.Context
{
    public class BbDbContext : DbContext
    {
        public BbDbContext(DbContextOptions<BbDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuario { get; set; } = null!;
        public DbSet<Categoria> Categoria { get; set; } = null!;
        public DbSet<Livro> Livro { get; set; } = null!;
        public DbSet<LivroCategoria> LivroCategoria { get; set; } = null!;
        public DbSet<Reserva> Reserva { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LivroCategoria>()
                .HasKey(lc => new { lc.LivroId, lc.CategoriaId });
        }
    }
}