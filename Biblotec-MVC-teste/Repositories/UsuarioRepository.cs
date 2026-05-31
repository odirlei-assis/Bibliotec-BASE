using Bibliotec_MVC_teste.Models;
using Biblotec_MVC_teste.Context;
using Biblotec_MVC_teste.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Biblotec_MVC_teste.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly BbDbContext _context;

        public UsuarioRepository(BbDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> BuscarPorEmailSenha(string email, string senha)
        {
            return await _context.Usuario.FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);
        }
    }
}