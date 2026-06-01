using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bibliotec_MVC_teste.Models;

namespace Biblotec_MVC_teste.Interfaces
{
    public interface ILivroRepository
    {
        Task<IEnumerable<Livro>> BuscarLivrosAsync();

        Task<Livro?> BuscarLivroPorId(int id);

        Task DeletarLivroAsync(Livro l);

        Task RemoverCatDoLivroAsync(int id);

        Task EditarLivroAsync(Livro l);

        Task<Livro?> BuscarLivroComCatPorID(int id);

        Task RemoverLivroCatAsync(IEnumerable<LivroCategoria> livroCategorias);

        Task AdicionarLivroCatAsync(LivroCategoria livroCategoria);
    }
}