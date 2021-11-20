using Google.Cloud.Firestore;
using System.Windows.Forms;

namespace KargoTakipUygulamasi
{
   
    [FirestoreData]
    public class Users
    {
        [FirestoreProperty]
        public int UserId { get; set; }
        [FirestoreProperty]
        
        public string Username { get; set; }
        [FirestoreProperty]
        public string Password { get; set; }

    
    }
}

