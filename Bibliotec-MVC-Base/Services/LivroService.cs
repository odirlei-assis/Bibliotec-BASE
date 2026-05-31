using Bibliotec_MVC_Base.Entities;
using Bibliotec_MVC_Base.Interfaces;

using Microsoft.AspNetCore.Http;

namespace Bibliotec_MVC_Base.Services;

public class LivroService : ILivroService
{
    private readonly ILivroRepository _livroRepository;

    public LivroService(ILivroRepository livroRepository)
    {
        _livroRepository = livroRepository;
    }

    public async Task<IEnumerable<Livro>> GetAllLivrosComCategoriasAsync()
    {
        return await _livroRepository.GetAllWithCategoriesAsync();
    }

    public async Task<IEnumerable<Categoria>> GetAllCategoriasAsync()
    {
        return await _livroRepository.GetAllCategoriasAsync();
    }

    public async Task<Livro?> GetLivroByIdComCategoriasAsync(int id)
    {
        return await _livroRepository.GetByIdWithCategoriesAsync(id);
    }

    public async Task<Livro?> GetLivroByIdAsync(int id)
    {
        return await _livroRepository.GetByIdAsync(id);
    }

    private async Task<string> ProcessImageUploadAsync(IFormFile arquivoImagem)
    {
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "livros");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(arquivoImagem.FileName);
        var filePath = Path.Combine(folderPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await arquivoImagem.CopyToAsync(stream);
        }

        return $"/img/livros/{fileName}";
    }

    public async Task AddLivroAsync(Livro livro, string? categoriasSelecionadas, IFormFile? arquivoImagem, string? ativo)
    {
        livro.Status = (ativo == "true" || ativo == "on") ? "D" : "I";

        if (arquivoImagem != null && arquivoImagem.Length > 0)
        {
            livro.Imagem = await ProcessImageUploadAsync(arquivoImagem);
        }
        else
        {
            livro.Imagem = "";
        }

        await _livroRepository.AddAsync(livro);

        if (!string.IsNullOrEmpty(categoriasSelecionadas))
        {
            var categoriaIds = categoriasSelecionadas.Split(',')
                .Select(id => int.TryParse(id, out var parsed) ? parsed : 0)
                .Where(id => id > 0)
                .ToList();

            foreach (var catId in categoriaIds)
            {
                var lc = new LivroCategoria
                {
                    LivroId = livro.Id,
                    CategoriaId = catId
                };
                await _livroRepository.AddLivroCategoriaAsync(lc);
            }
        }
    }

    public async Task UpdateLivroAsync(int id, Livro livroEditado, string? categoriasSelecionadas, IFormFile? arquivoImagem, string? ativo)
    {
        var livroExistente = await _livroRepository.GetByIdWithCategoriesAsync(id);
        if (livroExistente == null) throw new KeyNotFoundException("Livro não encontrado.");

        livroExistente.Titulo = livroEditado.Titulo;
        livroExistente.Autor = livroEditado.Autor;
        livroExistente.Editora = livroEditado.Editora;
        livroExistente.AnoPublicacao = livroEditado.AnoPublicacao;
        livroExistente.Sinopse = livroEditado.Sinopse;
        livroExistente.Status = (ativo == "true" || ativo == "on") ? "D" : "I";

        if (arquivoImagem != null && arquivoImagem.Length > 0)
        {
            livroExistente.Imagem = await ProcessImageUploadAsync(arquivoImagem);
        }

        await _livroRepository.UpdateAsync(livroExistente);

        await _livroRepository.RemoveLivroCategoriasAsync(livroExistente.LivroCategorias);

        if (!string.IsNullOrEmpty(categoriasSelecionadas))
        {
            var categoriaIds = categoriasSelecionadas.Split(',')
                .Select(cid => int.TryParse(cid, out var parsed) ? parsed : 0)
                .Where(cid => cid > 0)
                .ToList();

            foreach (var catId in categoriaIds)
            {
                var lc = new LivroCategoria
                {
                    LivroId = livroExistente.Id,
                    CategoriaId = catId
                };
                await _livroRepository.AddLivroCategoriaAsync(lc);
            }
        }
    }

    public async Task DeleteLivroAsync(int id)
    {
        await _livroRepository.RemoveCategoriasFromLivroAsync(id);
        await _livroRepository.DeleteAsync(id);
    }
}
