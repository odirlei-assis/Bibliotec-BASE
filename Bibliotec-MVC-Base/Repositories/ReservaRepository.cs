using Bibliotec_MVC_Base.Context;
using Bibliotec_MVC_Base.Entities;
using Bibliotec_MVC_Base.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bibliotec_MVC_Base.Repositories;

public class ReservaRepository : Repository<Reserva>, IReservaRepository
{
    public ReservaRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Reserva>> GetAllWithDetailsAsync()
    {
        return await _context.Reserva
            .Include(r => r.Livro)
            .Include(r => r.Aluno)
            .OrderBy(r => r.DataReserva)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reserva>> GetReservasByUsuarioIdAsync(int usuarioId)
    {
        return await _context.Reserva
            .Include(r => r.Livro)
            .Include(r => r.Aluno)
            .Where(r => r.AlunoId == usuarioId)
            .OrderBy(r => r.DataReserva)
            .ToListAsync();
    }
}
