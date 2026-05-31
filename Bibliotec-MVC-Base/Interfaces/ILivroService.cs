using Bibliotec_MVC_Base.Entities;

using Microsoft.AspNetCore.Http;

namespace Bibliotec_MVC_Base.Interfaces;

public interface ILivroService
{
    Task<IEnumerable<Livro>> GetAllLivrosComCategoriasAsync();
    Task<IEnumerable<Categoria>> GetAllCategoriasAsync();
    Task<Livro?> GetLivroByIdComCategoriasAsync(int id);
    Task<Livro?> GetLivroByIdAsync(int id);
    Task AddLivroAsync(Livro livro, string? categoriasSelecionadas, IFormFile? arquivoImagem, string? ativo);
    Task UpdateLivroAsync(int id, Livro livroEditado, string? categoriasSelecionadas, IFormFile? arquivoImagem, string? ativo);
    Task DeleteLivroAsync(int id);
}
