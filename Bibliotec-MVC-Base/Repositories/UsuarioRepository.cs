using Bibliotec_MVC_Base.Context;
using Bibliotec_MVC_Base.Entities;
using Bibliotec_MVC_Base.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bibliotec_MVC_Base.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _context.Usuario.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Usuario?> GetByMatriculaAsync(string matricula)
    {
        return await _context.Usuario.FirstOrDefaultAsync(u => u.Matricula == matricula);
    }
}
