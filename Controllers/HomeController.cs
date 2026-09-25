using gift_of_the_givers.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gift_of_the_givers.Controllers;
public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;
    public HomeController(ApplicationDbContext db) => _db = db;

    public IActionResult Index()
    {
        var incidents = _db.Incidents.OrderByDescending(i => i.StartDate).ToList();
        var centres = _db.ReliefCentres.ToList();
        ViewBag.Incidents = incidents;
        ViewBag.Centres = centres;
        ViewBag.ActiveCount = incidents.Count(i => i.Status == "Active");
        ViewBag.TotalCapacity = centres.Sum(c => c.Capacity).ToString("N0");
        return View();
    }
    public IActionResult About() => View();
    public IActionResult Contact() => View();
}