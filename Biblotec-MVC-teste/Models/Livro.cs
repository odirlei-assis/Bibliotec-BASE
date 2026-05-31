using System.ComponentModel.DataAnnotations;

namespace Bibliotec_MVC_teste.Models;

public class Livro
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(150)]
    public string Titulo { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Autor { get; set; } = null!;

    public int AnoPublicacao { get; set; }

    [Required]
    [StringLength(1)]
    public string Status { get; set; } = null!; // 'D', 'E', 'I'

    public string? Sinopse { get; set; }

    [Required]
    [StringLength(50)]
    public string Editora { get; set; } = null!;

    public string? Imagem { get; set; }

    public ICollection<LivroCategoria> LivroCategorias { get; set; } = new List<LivroCategoria>();
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
