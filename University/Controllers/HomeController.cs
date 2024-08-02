using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using University.Models;
using Microsoft.EntityFrameworkCore;
using University.Data;
using University.Models.SchoolViewModels;
using Microsoft.Extensions.Logging;

namespace University.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    // Variable de clase para el contexto de la db
    private readonly SchoolContext _context;

    //public HomeController(ILogger<HomeController> logger)
    public HomeController(ILogger<HomeController> logger, SchoolContext context)
    {
        _logger = logger;
        _context = context; // add
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<ActionResult> About(){
        IQueryable<EnrollmentDateGroup> data = from student in _context.Students
                                               group student by student.EnrollmentDate into dateGroup
                                               select new EnrollmentDateGroup(){
                                                    EnrollmentDate = dateGroup.Key,
                                                    StudentCount = dateGroup.Count()
                                               };
        return View(await data.AsNoTracking().ToListAsync());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
