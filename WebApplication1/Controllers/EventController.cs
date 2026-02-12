using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class EventController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Buy(int eventId = 0)
        {
            return Content("ok you're seeing info about event: " + eventId);
        }
    }
}
