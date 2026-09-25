using gift_of_the_givers.Data;
using gift_of_the_givers.Models;
using Microsoft.AspNetCore.Mvc;

namespace gift_of_the_givers.Controllers;

public class VolunteersController : Controller
{
    private readonly ApplicationDbContext _db;
    public VolunteersController(ApplicationDbContext db) => _db = db;

    public IActionResult Index() => View(new Volunteer());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(Volunteer v)
    {
        if (!ModelState.IsValid) return View("Index", v);
        v.Status = "Available"; v.RegisteredDate = DateTime.UtcNow;
        _db.Volunteers.Add(v); await _db.SaveChangesAsync();
        ViewBag.Success = "Thanks — your registration has been recorded. A coordinator will contact you within 2–3 working days.";
        return View("Index", new Volunteer());
    }
}