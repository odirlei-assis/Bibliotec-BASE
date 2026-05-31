using Bibliotec_MVC_Base.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Bibliotec_MVC_Base.Entities;

namespace Bibliotec_MVC_Base.Controllers;

public class LivroController : Controller
{
    private readonly ILivroService _livroService;

    public LivroController(ILivroService livroService)
    {
        _livroService = livroService;
    }

    public async Task<IActionResult> Index()
    {
        var adminSession = HttpContext.Session.GetString("Admin");
        if (adminSession == null)
        {
            return RedirectToAction("Index", "Login");
        }

        ViewBag.Admin = adminSession == "True" || adminSession == "true";

        var livros = await _livroService.GetAllLivrosComCategoriasAsync();

        ViewBag.Categorias = await _livroService.GetAllCategoriasAsync();

        return View(livros);
    }

    [HttpGet]
    public async Task<IActionResult> Cadastro()
    {
        var adminSession = HttpContext.Session.GetString("Admin");
        if (adminSession == null || (adminSession != "True" && adminSession != "true"))
        {
            return RedirectToAction("Index"); // Somente Admin pode cadastrar
        }

        ViewBag.Admin = true;
        ViewBag.Categorias = await _livroService.GetAllCategoriasAsync();

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Cadastro(Livro livro, string? CategoriasSelecionadas, IFormFile? ArquivoImagem, string? Ativo)
    {
        var adminSession = HttpContext.Session.GetString("Admin");
        if (adminSession == null || (adminSession != "True" && adminSession != "true"))
        {
            return RedirectToAction("Index"); // Somente Admin pode cadastrar
        }

        await _livroService.AddLivroAsync(livro, CategoriasSelecionadas, ArquivoImagem, Ativo);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Editar(Livro livroEditado, string? CategoriasSelecionadas, IFormFile? ArquivoImagem, string? Ativo)
    {
        var adminSession = HttpContext.Session.GetString("Admin");
        if (adminSession == null || (adminSession != "True" && adminSession != "true"))
        {
            return Unauthorized();
        }

        try
        {
            await _livroService.UpdateLivroAsync(livroEditado.Id, livroEditado, CategoriasSelecionadas, ArquivoImagem, Ativo);
            return RedirectToAction("Index");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Excluir(int id)
    {
        var adminSession = HttpContext.Session.GetString("Admin");
        if (adminSession == null || (adminSession != "True" && adminSession != "true"))
        {
            return Unauthorized();
        }

        var livro = await _livroService.GetLivroByIdAsync(id);
        if (livro == null)
        {
            return NotFound();
        }

        await _livroService.DeleteLivroAsync(id);

        return Ok();
    }
}
