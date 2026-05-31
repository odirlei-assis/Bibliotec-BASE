using Bibliotec_MVC_Base.Entities;
using Bibliotec_MVC_Base.Interfaces;

namespace Bibliotec_MVC_Base.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task AddUsuarioAsync(Usuario usuario)
    {
        await _usuarioRepository.AddAsync(usuario);
    }

    public async Task<Usuario?> AuthenticateAsync(string email, string senha)
    {
        var user = await _usuarioRepository.GetByEmailAsync(email);
        if (user != null && user.Senha == senha)
        {
            return user;
        }
        return null;
    }

    public async Task DeleteUsuarioAsync(int id)
    {
        await _usuarioRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Usuario>> GetAllUsuariosAsync()
    {
        return await _usuarioRepository.GetAllAsync();
    }

    public async Task<Usuario?> GetUsuarioByIdAsync(int id)
    {
        return await _usuarioRepository.GetByIdAsync(id);
    }

    public async Task UpdateUsuarioAsync(Usuario usuario)
    {
        await _usuarioRepository.UpdateAsync(usuario);
    }
}
