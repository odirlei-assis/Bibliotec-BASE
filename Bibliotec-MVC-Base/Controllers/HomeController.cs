using Microsoft.AspNetCore.Mvc;

namespace Bibliotec_MVC_Base.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
