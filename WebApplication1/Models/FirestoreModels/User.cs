using Google.Cloud.Firestore;

namespace WebApplication1.Models.FirestoreModels
{
    [FirestoreData]
    public class User
    {

        [FirestoreProperty]
        public string Email { get; set; }
        [FirestoreProperty]
        public string FirstName { get; set; }

        [FirestoreProperty]
        public string LastName { get; set; }

        [FirestoreProperty]
        public string MobileNumber { get; set; }



    }
}
