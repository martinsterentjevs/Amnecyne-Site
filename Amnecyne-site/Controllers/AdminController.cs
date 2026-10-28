using AspNet.Security.OAuth.GitHub;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amnecyne.Controllers;

[Authorize]
[Route("admin")]
public class AdminController : Controller
{
    [HttpGet("")]
    public IActionResult Index() => View();
}

[AllowAnonymous]
[Route("login")]
public class AuthController : Controller
{
    [HttpGet("")]
    public IActionResult Login() =>
        Challenge(new AuthenticationProperties { RedirectUri = "/admin" }, GitHubAuthenticationDefaults.AuthenticationScheme);

    [HttpGet("/logout")]
    [HttpPost("/logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}