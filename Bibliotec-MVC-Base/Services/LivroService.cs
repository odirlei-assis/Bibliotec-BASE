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
        // Busca o livro existente no banco junto com suas categorias
        var livroExistente = await _livroRepository.GetByIdWithCategoriesAsync(id);

        // Se não encontrar o livro, interrompe a execução
        if (livroExistente == null)
            throw new KeyNotFoundException("Livro não encontrado.");

        // Atualiza os dados básicos do livro
        livroExistente.Titulo = livroEditado.Titulo;
        livroExistente.Autor = livroEditado.Autor;
        livroExistente.Editora = livroEditado.Editora;
        livroExistente.AnoPublicacao = livroEditado.AnoPublicacao;
        livroExistente.Sinopse = livroEditado.Sinopse;

        // Define o status do livro:
        // D = Disponível | I = Indisponível
        livroExistente.Status = (ativo == "true" || ativo == "on") ? "D" : "I";

        // Se uma nova imagem foi enviada, faz o upload e atualiza o caminho da imagem
        if (arquivoImagem != null && arquivoImagem.Length > 0)
        {
            livroExistente.Imagem = await ProcessImageUploadAsync(arquivoImagem);
        }

        // Salva as alterações do livro
        await _livroRepository.UpdateAsync(livroExistente);

        // Remove todos os relacionamentos atuais entre o livro e suas categorias
        await _livroRepository.RemoveLivroCategoriasAsync(livroExistente.LivroCategorias);

        // Verifica se foram enviadas categorias
        if (!string.IsNullOrEmpty(categoriasSelecionadas))
        {
            // Converte a string "1,2,3" em uma lista de inteiros
            var categoriaIds = categoriasSelecionadas.Split(',')
                .Select(cid => int.TryParse(cid, out var parsed) ? parsed : 0)
                .Where(cid => cid > 0)
                .ToList();

            // Cria novamente os relacionamentos Livro x Categoria
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
