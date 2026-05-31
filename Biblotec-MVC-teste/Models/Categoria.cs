using System.ComponentModel.DataAnnotations;

namespace Bibliotec_MVC_teste.Models;

public class Categoria
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Nome { get; set; } = null!;

    public ICollection<LivroCategoria> LivroCategorias { get; set; } = new List<LivroCategoria>();
}
