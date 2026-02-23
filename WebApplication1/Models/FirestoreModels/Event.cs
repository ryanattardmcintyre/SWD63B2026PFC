using Google.Cloud.Firestore;

namespace WebApplication1.Models.FirestoreModels
{
    [FirestoreData]
    public class Event
    {
        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public string ImagePath { get; set; }

        [FirestoreProperty]
        public DateTime DateTimeHappening { get; set; }
    }
}
