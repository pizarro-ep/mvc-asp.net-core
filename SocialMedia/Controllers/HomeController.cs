using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Models;
using Microsoft.AspNetCore.Authorization;

namespace SocialMedia.Controllers;

[Authorize(Roles = "Administrador, Moderador")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [AllowAnonymous] 
    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult NonSecuredMethod(){
        return View();
    }

    [Authorize]
    public IActionResult SecuredMethod(){
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
