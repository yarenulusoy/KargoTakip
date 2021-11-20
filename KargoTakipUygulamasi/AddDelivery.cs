using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Cloud.Firestore;


namespace KargoTakipUygulamasi
{
    public partial class AddDelivery : Form
    {
        public AddDelivery()
        {
            InitializeComponent();

        }
        FirestoreDb db;

        public void Database()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"kargo-takip.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("firebase-name");

        }
        private void AddDelivery_Load(object sender, EventArgs e)
        {

            Database();

            GMapProviders.GoogleMap.ApiKey = @"yourapikey";
            gMap.DragButton = MouseButtons.Left; //hareket ettirme
            gMap.MapProvider = GMapProviders.GoogleMap; //haritayı tanımlama
            gMap.ShowCenter = false;
            gMap.MinZoom = 10;
            gMap.MaxZoom = 18;
            gMap.SetPositionByKeywords("Izmit");
            gMap.Zoom = 14;



        }

        private void gMap_MouseClick(object sender, MouseEventArgs e)
        { 
         if (e.Button == MouseButtons.Left)
            {
                if (gMap.Overlays.Count>0)
                {
                    gMap.Overlays.RemoveAt(0);
                }
                var point = gMap.FromLocalToLatLng(e.X, e.Y);

                //  gMap.Position = point;
                LoadMap(point);
                AddMarker(point);

                //adresi getir
                var address = GetAddress(point);
                if (address != null)
                {
                    txtAdres.Text = String.Join(", ", address[0]);

                }
                else
                {
                    txtAdres.Text = "Unable";
                }

            }
        }

        private void LoadMap(PointLatLng point)
        {
            gMap.Position = point;
        }
        private void AddMarker(PointLatLng pointToAdd, GMarkerGoogleType markerType = GMarkerGoogleType.arrow)
        {
            var markers = new GMapOverlay("markers");
            var marker = new GMarkerGoogle(pointToAdd, markerType);
            markers.Markers.Add(marker);
            gMap.Overlays.Add(markers);
        }

        private List<String> GetAddress(PointLatLng point)
        {
            List<Placemark> placemarks = null;
            var statusCode = GMapProviders.GoogleMap.GetPlacemarks(point, out placemarks);
            if (statusCode == GeoCoderStatusCode.OK && placemarks != null)
            {
                List<String> address = new List<string>();
                foreach (var placemark in placemarks)
                {
                    address.Add(placemark.Address);
                }
                return address;
            }
            return null;
        }

       
        private async void save_Click(object sender, EventArgs e)
        {
            GeoCoderStatusCode statusCode; 
            var pointLatLng = GoogleMapProvider.Instance.GetPoint(txtAdres.Text.Trim(), out statusCode);
            var pt = pointLatLng ?? default(PointLatLng);
            var placeMark = GoogleMapProvider.Instance.GetPlacemark(pt, out statusCode);
            
            CollectionReference docref = db.Collection("kargoTakip");
            Query query = db.Collection("kargoTakip");
            QuerySnapshot snapshot = await query.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
            {
                Customers customers = documentSnapshot.ConvertTo<Customers>();
                autoId = customers.KargoID;
            }
            autoId++;
            string adres= placeMark?.Address;
            Customers customer = new Customers
            {
                KargoID = autoId,
                MusteriAdi = txtAd.Text,
                Adres = adres,
                Durum = false,

            };

            db = FirestoreDb.Create("kargo-takip-uygulamasi");
            DocumentReference docRef = db.Collection("kargoTakip").Document("" + customer.KargoID + "");

            await docRef.SetAsync(customer);
            MessageBox.Show("Kayıt Eklendi.");
            this.Hide();
        }

        int autoId=0;

  
    }
}
