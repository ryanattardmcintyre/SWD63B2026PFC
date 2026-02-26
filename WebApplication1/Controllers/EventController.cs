using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.FirestoreModels;
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
        public async Task<IActionResult> Buy(string eventId, int qty =0)
        {
            try
            {
                string emailAddress = User.Claims.SingleOrDefault(x => x.Type.Contains("emailaddress")).Value;

                Ticket myTicket = new Ticket()
                {
                    Event = eventId,
                    PurchaseDate = DateTime.UtcNow,
                    Quantity = qty,
                    Status = "Paid",
                    UserEmail = emailAddress
                };

                await _firestoreRepository.AddTicketAsync(myTicket);

                TempData["success"] = "Tickets bought successfully";
            }
            catch (Exception ex)
            {
                TempData["error"] = "Tickets weren't bought. Try later"; 
                //Log on the cloud the actual error
            }

            return RedirectToAction("Index");
        }
    }
}
