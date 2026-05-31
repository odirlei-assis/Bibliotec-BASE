using System.ComponentModel.DataAnnotations;

namespace Bibliotec_MVC_teste.Models;

public class Usuario
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string Matricula { get; set; } = null!;

    public bool Ativo { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string Senha { get; set; } = null!;

    [Required]
    [StringLength(11)]
    public string NumCel { get; set; } = null!;

    public bool TipoBib { get; set; } // 0 = aluno, 1 = bibliotecaria

    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
