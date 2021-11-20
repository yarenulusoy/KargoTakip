using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KargoTakipUygulamasi
{
    public partial class MapGui : Form
    {

        public MapGui()
        {
            InitializeComponent();
            _points = new List<PointLatLng>();
        }
        private List<PointLatLng> _points;
        FirestoreDb db;
        public int gonder;

        public void Database()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"kargo-takip.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("firebase-name");

        }
        private void MapGui_Load(object sender, EventArgs e)
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
            GetData();
          



        }

        private void LoadMap(PointLatLng point)
        {
            gMap.Position = point;

        }
        public void AddMarker(PointLatLng pointToAdd, GMarkerGoogleType markerType = GMarkerGoogleType.arrow)
        {

            var markers = new GMapOverlay("markers");
            var marker = new GMarkerGoogle(pointToAdd, markerType);
            markers.Markers.Add(marker);
            gMap.Overlays.Add(markers);

        }

        private void RemoveMarker(PointLatLng point)
        {
            int i = 0; int a = 0;

            foreach (PointLatLng p in _points)
            {
                if (p == point)
                {
                    a = i;
                    i = 0;
                }
                else
                {
                    i++;
                }

            }
            _points.RemoveAt(a);
            gMap.Overlays.RemoveAt(a);
        }


        private async void GetData()
        {
            _points.Clear();
            GeoCoderStatusCode statusCode;
            CollectionReference citiesRef = db.Collection("kargoTakip");
            Query query = db.Collection("kargoTakip");


            FirestoreChangeListener listener = query.Listen(snapshot =>
            {
                foreach (DocumentChange change in snapshot.Changes)
                {
                    if (change.ChangeType.ToString() == "Added")
                    {


                        Customers customer = change.Document.ConvertTo<Customers>();
                        this.Invoke(new Action(() =>
                        {
                            var pointLatLng = GoogleMapProvider.Instance.GetPoint(customer.Adres.Trim(), out statusCode);

                            if (statusCode == GeoCoderStatusCode.OK)
                            {
                                if (customer.Durum == false)
                                {
                                    var pt = pointLatLng ?? default(PointLatLng);
                                    _points.Add(pt);
                                    LoadMap(pt);
                                    AddMarker(pt);
                                    
                                    gMap.Refresh();
                                    

                                }

                            }
                            else
                            {
                                MessageBox.Show("Hata Olustu");
                            }
                        }));
                    }
                    else if (change.ChangeType.ToString() == "Modified")
                    {
                        Customers customer = change.Document.ConvertTo<Customers>();
                        this.Invoke(new Action(() =>
                        {
                            var pointLatLng = GoogleMapProvider.Instance.GetPoint(customer.Adres.Trim(), out statusCode);

                            if (statusCode == GeoCoderStatusCode.OK)
                            {
                                var pt = pointLatLng ?? default(PointLatLng);
                                if (customer.Durum == true)
                                {
                                    RemoveMarker(pt);
                                }


                            }
                            else
                            {
                                MessageBox.Show("Hata Olustu");
                            }
                        }));
                    }
                    else if (change.ChangeType.ToString() == "Removed")
                    {

                        Customers customer = change.Document.ConvertTo<Customers>();
                        this.Invoke(new Action(() =>
                        {
                            var pointLatLng = GoogleMapProvider.Instance.GetPoint(customer.Adres.Trim(), out statusCode);

                            if (statusCode == GeoCoderStatusCode.OK)
                            {
                                var pt = pointLatLng ?? default(PointLatLng);
                                RemoveMarker(pt);

                            }
                            else
                            {
                                MessageBox.Show("Hata Olustu");
                            }
                        }));
                    }
                }
            });

            await Task.Delay(1000);

        }


        List<int> routeOverlays = new List<int>();




        public void distance()
        {
            GDirections ss;
            for (int i = 0; i < _points.Count; i++)
            {
                if (i + 1 < _points.Count)
                {
                    GMapProviders.GoogleMap.GetDirections(out ss, _points[i], _points[i + 1], 
                        false, false, false, false, false);
                    var r = new GMapRoute(ss.Route, "My Route")
                    {
                        Stroke = new Pen(Color.Red, 5)
                    };


                    var routes = new GMapOverlay("routes");
                    routes.Routes.Add(r);
                    gMap.Overlays.Add(routes);
                    gMap.Refresh();
                    routeOverlays.Add(gMap.Overlays.Count - 1);

                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            distance();
        }
    }

}

