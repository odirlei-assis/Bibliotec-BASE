using Bibliotec_MVC_Base.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Bibliotec_MVC_Base.Entities;

namespace Bibliotec_MVC_Base.Controllers;

public class ReservaController : Controller
{
    private readonly IReservaService _reservaService;

    public ReservaController(IReservaService reservaService)
    {
        _reservaService = reservaService;
    }

    public async Task<IActionResult> Index()
    {
        var adminSession = HttpContext.Session.GetString("Admin");
        var usuarioIdStr = HttpContext.Session.GetString("UsuarioID");

        if (adminSession == null || string.IsNullOrEmpty(usuarioIdStr) || !int.TryParse(usuarioIdStr, out int usuarioId))
        {
            return RedirectToAction("Index", "Login");
        }

        ViewBag.Admin = adminSession == "True" || adminSession == "true";

        IEnumerable<Reserva> reservas;
        if (ViewBag.Admin)
        {
            reservas = await _reservaService.GetAllReservasComDetalhesAsync();
        }
        else
        {
            reservas = await _reservaService.GetReservasByUsuarioIdAsync(usuarioId);
        }

        return View(reservas);
    }

    [HttpPost]
    public async Task<IActionResult> Excluir(int id)
    {
        var adminSession = HttpContext.Session.GetString("Admin");
        var usuarioIdStr = HttpContext.Session.GetString("UsuarioID");

        if (adminSession == null || string.IsNullOrEmpty(usuarioIdStr) || !int.TryParse(usuarioIdStr, out int usuarioId))
        {
            return Unauthorized();
        }

        var isAdmin = adminSession == "True" || adminSession == "true";

        try
        {
            await _reservaService.DeleteReservaAsync(id, usuarioId, isAdmin);
            return Ok(new { mensagem = "Reserva excluída com sucesso!" });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Editar(int Id, DateTime? DataEmprestimo, DateTime? DataPrevistaDevolucao, string? DanoLivro, string Status)
    {
        var adminSession = HttpContext.Session.GetString("Admin");
        if (adminSession == null || (adminSession != "True" && adminSession != "true"))
        {
            return Unauthorized();
        }

        try
        {
            await _reservaService.UpdateReservaAsync(Id, DataEmprestimo, DataPrevistaDevolucao, DanoLivro, Status);
            return RedirectToAction("Index");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Reservar(int LivroId)
    {
        var usuarioIdStr = HttpContext.Session.GetString("UsuarioID");
        if (string.IsNullOrEmpty(usuarioIdStr) || !int.TryParse(usuarioIdStr, out int usuarioId))
        {
            return Unauthorized();
        }

        var novaReserva = new Reserva
        {
            LivroId = LivroId,
            AlunoId = usuarioId,
            DataReserva = DateTime.Now,
            Status = "E" // E = Espera
        };

        try
        {
            await _reservaService.AddReservaAsync(novaReserva);
            return Ok(new { mensagem = "Reserva criada com sucesso!" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }
}
