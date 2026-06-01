using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bibliotec_MVC_teste.Models;
using Biblotec_MVC_teste.Interfaces;

namespace Biblotec_MVC_teste.Repositories
{
    public class LivroService : ILivroService
    {
        private readonly ILivroRepository _livroRepository;

        public LivroService(ILivroRepository livroRepository)
        {
            _livroRepository = livroRepository;
        }

        private async Task<string> ProcessImageUploadAsync(IFormFile arquivoImagem)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "capa-livros");
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

        public async Task AtualizarLivroAsync(int id, Livro livroEditado, string? categoriasSelecionadas, IFormFile? arquivoImagem, string? ativo)
        {
            // Busca o livro existente no banco junto com suas categorias
            var livroExistente = await _livroRepository.BuscarLivroComCatPorID(id);

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
            await _livroRepository.EditarLivroAsync(livroExistente);

            // Remove todos os relacionamentos atuais entre o livro e suas categorias
            await _livroRepository.RemoverLivroCatAsync(livroExistente.LivroCategorias);

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

                    await _livroRepository.AdicionarLivroCatAsync(lc);
                }
            }
        }

        public async Task<IEnumerable<Livro>> BuscarLivrosComCatAsync()
        {
            return await _livroRepository.BuscarLivrosAsync();
        }

        public async Task<bool> RemoverLivroAsync(int id)
        {
            Livro? l = await _livroRepository.BuscarLivroPorId(id);

            if (l == null)
                return false;

            await _livroRepository.RemoverCatDoLivroAsync(id);
            await _livroRepository.DeletarLivroAsync(l);

            return true;
        }
    }
}