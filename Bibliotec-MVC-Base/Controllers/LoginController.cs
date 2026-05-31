using Bibliotec_MVC_Base.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bibliotec_MVC_Base.Controllers;

public class LoginController : Controller
{
    private readonly IUsuarioService _usuarioService;

    public LoginController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logar(string email, string senha)
    {
        var usuario = await _usuarioService.AuthenticateAsync(email, senha);
        if (usuario != null)
        {
            HttpContext.Session.SetString("UsuarioID", usuario.Id.ToString());
            HttpContext.Session.SetString("Admin", usuario.TipoBib.ToString());
            return RedirectToAction("Index", "Livro");
        }

        ViewBag.Erro = "Usuário ou senha inválidos.";
        return View("Index");
    }

    public IActionResult Deslogar()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}
