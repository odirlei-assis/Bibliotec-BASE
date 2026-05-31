using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Biblotec_MVC_teste.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

}
