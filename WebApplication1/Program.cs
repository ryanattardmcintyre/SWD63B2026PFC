using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Text.Json;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// Load Google JSON file
var googleJsonPath = Path.Combine(
    builder.Environment.ContentRootPath,
    "clientsecrets.json"
);

var googleConfig = JsonSerializer.Deserialize<GoogleOAuthConfig>(
    File.ReadAllText(googleJsonPath)
)!;

// Add services
builder.Services.AddControllersWithViews();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddGoogle(options =>
    {
        options.ClientId = googleConfig.web.client_id;
        options.ClientSecret = googleConfig.web.client_secret;

        options.SaveTokens = true;
        options.Scope.Add("email");
        options.Scope.Add("profile");
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
