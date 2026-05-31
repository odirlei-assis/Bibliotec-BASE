using Bibliotec_MVC_Base.Context;
using Bibliotec_MVC_Base.Entities;
using Bibliotec_MVC_Base.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bibliotec_MVC_Base.Repositories;

public class LivroRepository : Repository<Livro>, ILivroRepository
{
    public LivroRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Livro>> GetAllWithCategoriesAsync()
    {
        return await _context.Livro
            .Include(l => l.LivroCategorias)
            .ThenInclude(lc => lc.Categoria)
            .ToListAsync();
    }

    public async Task<IEnumerable<Categoria>> GetAllCategoriasAsync()
    {
        return await _context.Categoria.ToListAsync();
    }

    public async Task<Livro?> GetByIdWithCategoriesAsync(int id)
    {
        return await _context.Livro
            .Include(l => l.LivroCategorias)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task AddLivroCategoriaAsync(LivroCategoria livroCategoria)
    {
        await _context.LivroCategoria.AddAsync(livroCategoria);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveLivroCategoriasAsync(IEnumerable<LivroCategoria> livroCategorias)
    {
        _context.LivroCategoria.RemoveRange(livroCategorias);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveCategoriasFromLivroAsync(int livroId)
    {
        var categorias = _context.LivroCategoria.Where(lc => lc.LivroId == livroId);
        _context.LivroCategoria.RemoveRange(categorias);
        await _context.SaveChangesAsync();
    }
}
