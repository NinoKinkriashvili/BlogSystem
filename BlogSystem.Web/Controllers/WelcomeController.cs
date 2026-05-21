using Microsoft.AspNetCore.Mvc;

namespace BlogSystem.Web.Controllers;

public class WelcomeController : Controller
{
    public IActionResult Index() => View();
}
