using Bibliotec_MVC_Base.Entities;

namespace Bibliotec_MVC_Base.Interfaces;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> GetByMatriculaAsync(string matricula);
    Task<Usuario?> GetByEmailAsync(string email);
}
