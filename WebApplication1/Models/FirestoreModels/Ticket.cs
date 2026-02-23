using Google.Cloud.Firestore;

namespace WebApplication1.Models.FirestoreModels
{
    [FirestoreData]
    public class Ticket
    {
        [FirestoreProperty]
        public string Id { get; set; }
        
        [FirestoreProperty]
        public string Event { get; set; }
        
        [FirestoreProperty]
        public string UserEmail { get; set; }
        
        [FirestoreProperty]
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        [FirestoreProperty]
        public int Quantity { get; set; }
        
        [FirestoreProperty]
        public string Status { get; set; }
    }
}
