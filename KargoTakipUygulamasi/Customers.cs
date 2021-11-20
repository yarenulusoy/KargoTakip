using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace KargoTakipUygulamasi
{
    [FirestoreData]
    public class Customers
    {
        [FirestoreProperty] 
        public int KargoID { get; set; }
       
        [FirestoreProperty]
        public string MusteriAdi { get; set; }

        [FirestoreProperty] 
        public string Adres { get; set; }

        [FirestoreProperty]
        public bool Durum { get; set; }

   

    }
}
