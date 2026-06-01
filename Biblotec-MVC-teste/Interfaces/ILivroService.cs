using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bibliotec_MVC_teste.Models;

namespace Biblotec_MVC_teste.Interfaces
{
    public interface ILivroService
    {
        Task<IEnumerable<Livro>> BuscarLivrosComCatAsync();

        Task<bool> RemoverLivroAsync(int id);

        Task AtualizarLivroAsync(int id, Livro livroEditado, string? categoriasSelecionadas, IFormFile? arquivoImagem, string? ativo);

    }
}