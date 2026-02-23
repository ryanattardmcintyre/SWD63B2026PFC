using Google.Cloud.Firestore;
using WebApplication1.Models.FirestoreModels;

namespace WebApplication1.Repositories
{
    public class FirestoreRepository
    {
        private FirestoreDb _firestoreDb;

        public FirestoreRepository(string projectId)
        {
            _firestoreDb = FirestoreDb.Create(projectId);
        }

        //Add User
        public async Task AddUserAsync(User user)
        {
            CollectionReference usersCollection = _firestoreDb.Collection("users");
            await usersCollection.Document(user.Email).SetAsync(user);
        }


        //Add Tickets under users
        public async Task AddTicketAsync(Ticket ticket)
        {
            DocumentReference userDocRef = _firestoreDb.Collection("users").Document(ticket.UserEmail);
            ticket.PurchaseDate = DateTime.UtcNow; // Set purchase date to current time
            await userDocRef.Collection("tickets").Document().SetAsync(ticket);
        }

        //Get user by email
        public async Task<User> GetUserByEmailAsync(string email)
        {
            DocumentReference userDocRef = _firestoreDb.Collection("users").Document(email);
            DocumentSnapshot userSnapshot = await userDocRef.GetSnapshotAsync();

            if (userSnapshot.Exists)
            {
                return userSnapshot.ConvertTo<User>();
            }
            else
            {
                return null; // User not found
            }
        }

        //Get all events
        public async Task<List<Event>> GetAllEventsAsync()
        {
            // Get all Events
            CollectionReference eventsCollection = _firestoreDb.Collection("events");
            QuerySnapshot eventsSnapshot = await eventsCollection.GetSnapshotAsync();

            List<Event> events = new List<Event>();
            foreach (DocumentSnapshot doc in eventsSnapshot.Documents)
            {
                var e = doc.ConvertTo<Event>();
                e.Id = doc.Id;

                events.Add(e);
            }
            return events;
        }


        //Get all tickets for a user
        public async Task<List<Ticket>> GetTicketsByUserEmailAsync(string email)
        {
            DocumentReference userDocRef = _firestoreDb.Collection("users").Document(email);
            CollectionReference ticketsCollection = userDocRef.Collection("tickets");
            QuerySnapshot ticketsSnapshot = await ticketsCollection.GetSnapshotAsync();
            List<Ticket> tickets = new List<Ticket>();
            foreach (DocumentSnapshot doc in ticketsSnapshot.Documents)
            {
                tickets.Add(doc.ConvertTo<Ticket>());
            }
            return tickets;

        }
    }
}
