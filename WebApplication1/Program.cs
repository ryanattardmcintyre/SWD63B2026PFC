using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Text.Json;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", Path.Combine(
    builder.Environment.ContentRootPath,
    "swd63bpfc2026-80ca4a71595c.json"
));




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

builder.Services.AddScoped<WebApplication1.Repositories.FirestoreRepository>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var projectId = config.GetValue<string>("ProjectId");
    return new WebApplication1.Repositories.FirestoreRepository(projectId);
});


var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
