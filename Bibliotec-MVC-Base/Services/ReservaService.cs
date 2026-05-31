using Bibliotec_MVC_Base.Entities;
using Bibliotec_MVC_Base.Interfaces;

namespace Bibliotec_MVC_Base.Services;

public class ReservaService : IReservaService
{
    private readonly IReservaRepository _reservaRepository;

    public ReservaService(IReservaRepository reservaRepository)
    {
        _reservaRepository = reservaRepository;
    }

    public async Task<IEnumerable<Reserva>> GetAllReservasComDetalhesAsync()
    {
        return await _reservaRepository.GetAllWithDetailsAsync();
    }

    public async Task<Reserva?> GetReservaByIdAsync(int id)
    {
        return await _reservaRepository.GetByIdAsync(id);
    }

    public async Task AddReservaAsync(Reserva reserva)
    {
        var reservasAtuais = await _reservaRepository.GetReservasByUsuarioIdAsync(reserva.AlunoId);
        if (reservasAtuais.Any(r => r.LivroId == reserva.LivroId && r.Status != "F"))
        {
            throw new InvalidOperationException("Você já possui uma reserva ativa para este livro.");
        }

        await _reservaRepository.AddAsync(reserva);
    }

    public async Task UpdateReservaAsync(int id, DateTime? dataEmprestimo, DateTime? dataPrevistaDevolucao, string? danoLivro, string status)
    {
        var reserva = await _reservaRepository.GetByIdAsync(id);
        if (reserva == null)
            throw new KeyNotFoundException("Reserva não encontrada.");

        reserva.DataEmprestimo = dataEmprestimo;
        reserva.DataPrevistaDevolucao = dataPrevistaDevolucao;
        reserva.DanoLivro = danoLivro;
        reserva.Status = status;

        await _reservaRepository.UpdateAsync(reserva);
    }

    public async Task<IEnumerable<Reserva>> GetReservasByUsuarioIdAsync(int usuarioId)
    {
        return await _reservaRepository.GetReservasByUsuarioIdAsync(usuarioId);
    }

    public async Task DeleteReservaAsync(int id, int usuarioId, bool isAdmin)
    {
        var reserva = await _reservaRepository.GetByIdAsync(id);
        if (reserva == null)
            throw new KeyNotFoundException("Reserva não encontrada.");

        if (!isAdmin && reserva.AlunoId != usuarioId)
            throw new UnauthorizedAccessException("Você não tem permissão para excluir esta reserva.");

        await _reservaRepository.DeleteAsync(id);
    }
}
