using Bibliotec_MVC_teste.Models;
using Biblotec_MVC_teste.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Biblotec_MVC_teste.Controllers
{
    public class LivroController : Controller
    {
        private readonly ILivroService _livroService;

        public LivroController(ILivroService livroService)
        {
            _livroService = livroService;
        }

        public async Task<IActionResult> Index()
        {
            string? adminSession = HttpContext.Session.GetString("Admin");
            if (adminSession == null)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.Admin = adminSession == "True" || adminSession == "true";

            var livros = await _livroService.BuscarLivrosComCatAsync();

            return View(livros);
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(int id)
        {
            string? adminSession = HttpContext.Session.GetString("Admin");
            if (adminSession == null || (adminSession != "True" && adminSession != "true"))
                return Unauthorized();

            bool removido = await _livroService.RemoverLivroAsync(id);

            if (removido)
                return Ok();

            return NotFound();
        }
    }
}