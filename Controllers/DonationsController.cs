using gift_of_the_givers.Data;
using gift_of_the_givers.Models;
using Microsoft.AspNetCore.Mvc;

namespace gift_of_the_givers.Controllers;

public class DonationsController : Controller
{
    private readonly ApplicationDbContext _db;
    public DonationsController(ApplicationDbContext db) => _db = db;

    public IActionResult Index() => View(new Donation());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Donation d)
    {
        if (d.IsAnonymous) d.DonorName = "Anonymous";
        if (!ModelState.IsValid) return View("Index", d);
        _db.Donations.Add(d); await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Certificate), new { id = d.DonationID });
    }

    public IActionResult Certificate(int id)
    {
        var d = _db.Donations.Find(id);
        if (d is null) return NotFound();
        return View(d);
    }
}