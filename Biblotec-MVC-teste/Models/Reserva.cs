using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bibliotec_MVC_teste.Models;

public class Reserva
{
    [Key]
    public int Id { get; set; }

    public DateTime DataReserva { get; set; }

    public DateTime? DataEmprestimo { get; set; }

    public DateTime? DataPrevistaDevolucao { get; set; }

    [StringLength(255)]
    public string? DanoLivro { get; set; }

    [Required]
    [StringLength(1)]
    public string Status { get; set; } = null!; // E = Espera, P = Posse, A = Atraso, F = Finalizada

    public int AlunoId { get; set; }
    [ForeignKey("AlunoId")]
    public Usuario Aluno { get; set; } = null!;

    public int LivroId { get; set; }
    [ForeignKey("LivroId")]
    public Livro Livro { get; set; } = null!;
}
