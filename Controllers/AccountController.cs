using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace gift_of_the_givers.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _users;
    private readonly SignInManager<IdentityUser> _signIn;
    public AccountController(UserManager<IdentityUser> u, SignInManager<IdentityUser> s) { _users = u; _signIn = s; }

    public IActionResult Login() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string password)
    {
        var r = await _signIn.PasswordSignInAsync(email, password, false, false);
        if (r.Succeeded) return RedirectToAction("Index", "Home");
        ViewBag.Error = "Invalid username or password.";
        return View();
    }

    public IActionResult Register() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(string email, string password)
    {
        var user = new IdentityUser { UserName = email, Email = email };
        var r = await _users.CreateAsync(user, password);
        if (r.Succeeded)
        {
            await _users.AddToRoleAsync(user, "Donor");
            await _signIn.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }
        foreach (var e in r.Errors) ModelState.AddModelError("", e.Description);
        return View();
    }

    public async Task<IActionResult> Logout() { await _signIn.SignOutAsync(); return RedirectToAction("Index", "Home"); }
    public IActionResult AccessDenied() => View();
}