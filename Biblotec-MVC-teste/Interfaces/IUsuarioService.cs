using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bibliotec_MVC_teste.Models;

namespace Biblotec_MVC_teste.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario?> AutenticarUsuario(string email, string senha);
    }
}