using Bibliotec_MVC_Base.Entities;

namespace Bibliotec_MVC_Base.Interfaces;

public interface IReservaService
{
    Task<IEnumerable<Reserva>> GetAllReservasComDetalhesAsync();
    Task<Reserva?> GetReservaByIdAsync(int id);
    Task<IEnumerable<Reserva>> GetReservasByUsuarioIdAsync(int usuarioId);
    Task AddReservaAsync(Reserva reserva);
    Task UpdateReservaAsync(int id, DateTime? dataEmprestimo, DateTime? dataPrevistaDevolucao, string? danoLivro, string status);
    Task DeleteReservaAsync(int id, int usuarioId, bool isAdmin);
}
