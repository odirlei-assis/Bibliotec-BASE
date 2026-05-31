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

        public async Task<IEnumerable<Livro>> BuscarLivrosComCatAsync()
        {
            return await _livroRepository.BuscarLivrosAsync();
        }
    }
}