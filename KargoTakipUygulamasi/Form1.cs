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
using Google.Cloud.Firestore;

namespace KargoTakipUygulamasi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        FirestoreDb db;

        public void Database()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"kargo-takip.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("kargo-takip-uygulamasi");

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Database();
            
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) &&
                   string.IsNullOrWhiteSpace(txtPassword.Text))
            {

               MessageBox.Show("Lutfen tum alanlari doldurun.");
               return;
            }

       
            Query query = db.Collection("login");
            QuerySnapshot snapshot = await query.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
            {
                Users user = documentSnapshot.ConvertTo<Users>();
                if (txtPassword.Text != user.Password && txtUsername.Text != user.Username)
                {
                    MessageBox.Show("Kullanıcı adı ve şifre eşleşmiyor");
                    return;
                }
                else
                {
                    Delivery delivery = new Delivery();
                    this.Hide();
                    delivery.Show();
                    return;
                }
            }
          

        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername2.Text) ||
                string.IsNullOrWhiteSpace(txtPassword2.Text) ||
                string.IsNullOrWhiteSpace(txtPassword3.Text))
            {
                MessageBox.Show("Lutfen tum alanlari doldurun.");
                return;
            }


            var users = new Users()
            {
                Username = txtUsername2.Text,
                Password = txtPassword2.Text,
            
            };
           
       
          
            if (txtPassword2.Text == txtPassword3.Text)
            {
                DocumentReference docRef = db.Collection("login").Document("" + users.UserId + "");
                db = FirestoreDb.Create("kargo-takip-uygulamasi");
                await docRef.SetAsync(users);
                MessageBox.Show("Kullanici eklendi.");
                panel1.Visible = true;
                panel2.Visible = false;


            }
            else
            {
                MessageBox.Show("Sifreler aynı degil.Tekrar deneyiniz.");
            }

        }

        private void btnSignup_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            panel1.Visible = false;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
            
        }

        private async void button2_Click(object sender, EventArgs e)
        {
          
            CollectionReference Ref = db.Collection("login");
            Query query = Ref.WhereEqualTo("Username", textBox3.Text);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                DocumentReference dcRef = db.Collection("login").Document(documentSnapshot.Id);
            Dictionary<string, object> initialData = new Dictionary<string, object>
            {
                {"Password",textBox1.Text }
            };
            await dcRef.UpdateAsync(initialData);

            }
            MessageBox.Show("Kaydedildi.");
            panel3.Visible = false;

        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel3.Visible=true;
        }
    }
}
