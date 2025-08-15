using Microsoft.AspNetCore.Mvc;

namespace TestProjectApi.Controllers;

public class LoggerController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}