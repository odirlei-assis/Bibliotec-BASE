using Bibliotec_MVC_Base.Entities;

namespace Bibliotec_MVC_Base.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<Usuario>> GetAllUsuariosAsync();
    Task<Usuario?> GetUsuarioByIdAsync(int id);
    Task<Usuario?> AuthenticateAsync(string email, string senha);
    Task AddUsuarioAsync(Usuario usuario);
    Task UpdateUsuarioAsync(Usuario usuario);
    Task DeleteUsuarioAsync(int id);
}
