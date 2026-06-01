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

        public async Task AdicionarLivroCatAsync(LivroCategoria livroCategoria)
        {
            await _context.LivroCategoria.AddAsync(livroCategoria);
            await _context.SaveChangesAsync();
        }

        public async Task<Livro?> BuscarLivroComCatPorID(int id)
        {
            return await _context.Livro
                .Include(l => l.LivroCategorias)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Livro?> BuscarLivroPorId(int id)
        {
            return await _context.Livro.FindAsync(id);
        }

        public async Task<IEnumerable<Livro>> BuscarLivrosAsync()
        {
            return await _context.Livro
                .Include(l => l.LivroCategorias)
                .ThenInclude(lc => lc.Categoria)
                .ToListAsync();
        }

        public async Task DeletarLivroAsync(Livro l)
        {
            _context.Livro.Remove(l);
            await _context.SaveChangesAsync();
        }

        public async Task EditarLivroAsync(Livro l)
        {
            _context.Livro.Update(l);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverCatDoLivroAsync(int id)
        {
            IEnumerable<LivroCategoria> cat = _context.LivroCategoria.Where(lc => lc.LivroId == id);

            _context.LivroCategoria.RemoveRange(cat);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverLivroCatAsync(IEnumerable<LivroCategoria> livroCategorias)
        {
            _context.LivroCategoria.RemoveRange(livroCategorias);
            await _context.SaveChangesAsync();
        }
    }
}