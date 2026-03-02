using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.FirestoreModels;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    public class EventController : Controller
    {
        private FirestoreRepository _firestoreRepository;
        private BucketRepository _bucketRepository;
        public EventController(FirestoreRepository firestoreRepository, BucketRepository bucketRepository)
        {_bucketRepository  = bucketRepository;
            _firestoreRepository = firestoreRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Event e, IFormFile poster, IFormFile guestList)
        {
            //this gives you a google compatible datetime =  Timestamp.FromDateTime(e.DateTimeHappening)
            e.DateTimeHappening = DateTime.SpecifyKind(e.DateTimeHappening, DateTimeKind.Utc);


            string uniqueFilename = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(poster.FileName);
            Task<string> t =_bucketRepository.UploadFileAsync(poster, uniqueFilename);
            t.Wait();
            string pathToPoster = t.Result;
            if (!string.IsNullOrEmpty(pathToPoster))
            {   e.ImagePath = pathToPoster; 
            }

            string uniqueGuestListFilename = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(guestList.FileName);
            //change the bucket using KeyedScoped in program.cs
            Task<string> t2 = _bucketRepository.UploadFileAsync(guestList, uniqueGuestListFilename);
            t2.Wait();
            string pathToGuestList = t2.Result;
            if (!string.IsNullOrEmpty(pathToGuestList))
            {
                e.GuestListPath = pathToGuestList;
            }

            _bucketRepository.AssignPermission(e.OrganizerEmailAddress, uniqueGuestListFilename);

            await _firestoreRepository.AddEventAsync(e);

            return RedirectToAction("Index");
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
