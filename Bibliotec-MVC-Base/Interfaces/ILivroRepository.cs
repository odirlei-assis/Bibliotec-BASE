using Bibliotec_MVC_Base.Entities;

namespace Bibliotec_MVC_Base.Interfaces;

public interface ILivroRepository : IRepository<Livro>
{
    Task<IEnumerable<Livro>> GetAllWithCategoriesAsync();
    Task<IEnumerable<Categoria>> GetAllCategoriasAsync();
    Task<Livro?> GetByIdWithCategoriesAsync(int id);
    Task AddLivroCategoriaAsync(LivroCategoria livroCategoria);
    Task RemoveLivroCategoriasAsync(IEnumerable<LivroCategoria> livroCategorias);
    Task RemoveCategoriasFromLivroAsync(int livroId);
}
