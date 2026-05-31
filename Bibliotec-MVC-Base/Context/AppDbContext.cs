using Bibliotec_MVC_Base.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bibliotec_MVC_Base.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuario { get; set; } = null!;
    public DbSet<Categoria> Categoria { get; set; } = null!;
    public DbSet<Livro> Livro { get; set; } = null!;
    public DbSet<LivroCategoria> LivroCategoria { get; set; } = null!;
    public DbSet<Reserva> Reserva { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LivroCategoria>()
            .HasKey(lc => new { lc.LivroId, lc.CategoriaId });

        modelBuilder.Entity<LivroCategoria>()
            .HasOne(lc => lc.Livro)
            .WithMany(l => l.LivroCategorias)
            .HasForeignKey(lc => lc.LivroId);

        modelBuilder.Entity<LivroCategoria>()
            .HasOne(lc => lc.Categoria)
            .WithMany(c => c.LivroCategorias)
            .HasForeignKey(lc => lc.CategoriaId);

        modelBuilder.Entity<Reserva>()
            .HasIndex(r => new { r.AlunoId, r.LivroId, r.DataReserva })
            .IsUnique();

        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Matricula)
            .IsUnique();

        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Categoria>()
            .HasIndex(c => c.Nome)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}
