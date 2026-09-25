using gift_of_the_givers.Data;
using gift_of_the_givers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gift_of_the_givers.Controllers;

[Authorize(Roles = "Employee")]
public class EmployeeController : Controller
{
    private readonly ApplicationDbContext _db;
    public EmployeeController(ApplicationDbContext db) => _db = db;

    public IActionResult Dashboard()
    {
        ViewBag.Incidents = _db.Incidents.OrderByDescending(i => i.StartDate).ToList();
        ViewBag.Volunteers = _db.Volunteers.OrderByDescending(v => v.RegisteredDate).ToList();
        ViewBag.Updates = _db.ReliefUpdates.Include(u => u.Incident).OrderByDescending(u => u.PostedAt).ToList();
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> PostUpdate(ReliefUpdate u)
    {
        if (!ModelState.IsValid) return RedirectToAction(nameof(Dashboard));
        u.PostedBy = User.Identity!.Name ?? "Employee";
        _db.ReliefUpdates.Add(u); await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Dashboard));
    }
}