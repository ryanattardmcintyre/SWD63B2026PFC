using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login(string? returnUrl = "/")
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult GoogleLogin(string? returnUrl = "/")
    {
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(GoogleResponse)),
            Items =
            {
                ["returnUrl"] = returnUrl ?? "/"
            }
        };

        return Challenge(props, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet]
    public async Task<IActionResult> GoogleResponse()
    {
        // At this point Google auth has completed and cookie sign-in should occur automatically
        // because DefaultScheme is Cookies.
        // We just redirect back.

        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (!result.Succeeded)
            return RedirectToAction(nameof(Login));

        var returnUrl = result.Properties?.Items.TryGetValue("returnUrl", out var ru) == true ? ru : "/";
        return LocalRedirect(returnUrl!);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }
}
