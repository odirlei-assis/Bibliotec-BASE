using Bibliotec_MVC_teste.Models;
using Biblotec_MVC_teste.Context;
using Biblotec_MVC_teste.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Biblotec_MVC_teste.Repositories
{
    public class LivroRepository : ILivroRepository
    {
        private readonly BbDbContext _context;

        public LivroRepository(BbDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Livro>> BuscarLivrosAsync()
        {
            return await _context.Livro
                .Include(l => l.LivroCategorias)
                .ThenInclude(lc => lc.Categoria)
                .ToListAsync();
        }
    }
}