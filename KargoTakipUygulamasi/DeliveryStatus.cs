using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KargoTakipUygulamasi
{
    public partial class DeliveryStatus : Form
    {
        public DeliveryStatus()
        {
            InitializeComponent();
            Thread thread = new Thread(() =>
            {
                var frm = new MapGui();
                frm.StartPosition = FormStartPosition.Manual;
                frm.Left = 800;
                frm.Top = 20;
                Application.Run(frm);
            });
            thread.TrySetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        FirestoreDb db;
        public static int gonder;

        public void Database()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"kargo-takip.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("firebase-name");

        }
        private void add_Click(object sender, EventArgs e)
        {
            AddDelivery add = new AddDelivery();
            add.ShowDialog();
        }

        private void DeliveryStatus_Load(object sender, EventArgs e)
        {

            Database();
            GetData();

          
        }

        public async void GetData()
        {
            string durum="";
            CollectionReference citiesRef = db.Collection("kargoTakip");
            Query query = db.Collection("kargoTakip");


            FirestoreChangeListener listener = query.Listen(snapshot =>
            {

                gridData.Invoke(new Action(() =>
                {
                    gridData.Rows.Clear();


                }));

                foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
                {

                    Customers customer = documentSnapshot.ConvertTo<Customers>();
                    if (gridData.InvokeRequired)
                    {

                        gridData.Invoke(new Action(() =>
                        {
                            if (customer.Durum==true)
                            {
                                durum = "Teslim edildi.";
                            }
                            else
                            {
                                durum = "Teslim edilmedi.";
                            }
                            gridData.Rows.Add(customer.KargoID, customer.MusteriAdi, customer.Adres, durum);


                        }));

                    }
                }
            });

            await Task.Delay(1000);

        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (gridData.SelectedRows.Count > 0)
            {
                string id = gridData.CurrentRow.Cells[0].Value.ToString();
                gridData.Rows.RemoveAt(gridData.SelectedRows[0].Index);
                DocumentReference docRef = db.Collection("kargoTakip").Document(id);
                await docRef.DeleteAsync();
            }
            else

            {
                MessageBox.Show("Silinecek Satırı Seçin");
            }
        }

        private async void btnOk_Click(object sender, EventArgs e)
        {
            if (gridData.SelectedRows.Count > 0)
            {
                string id = gridData.CurrentRow.Cells[0].Value.ToString();
                gridData.Rows.RemoveAt(gridData.SelectedRows[0].Index);
                DocumentReference docRef = db.Collection("kargoTakip").Document(id);
                Dictionary<string, object> data = new Dictionary<string, object>()
                {
                    { "Durum",true}
                };
                DocumentSnapshot snap = await docRef.GetSnapshotAsync();
                if (snap.Exists)
                {
                    await docRef.UpdateAsync(data);
                }
               
            }
            else
            {
                  MessageBox.Show("Satırı Seçin");
            }
        }

        private void gridData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
