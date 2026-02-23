using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    public class EventController : Controller
    {
        private FirestoreRepository _firestoreRepository;
        public EventController(FirestoreRepository firestoreRepository)
        {
            _firestoreRepository = firestoreRepository;
        }
        public async Task<IActionResult> Index()
        {
            var list = await _firestoreRepository.GetAllEventsAsync();
            return View(list);
        }

        [Authorize]
        public IActionResult Buy(int eventId = 0)
        {
            return Content("ok you're seeing info about event: " + eventId);
        }
    }
}
