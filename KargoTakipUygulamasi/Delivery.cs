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
    public partial class Delivery : Form
    {
        public Delivery()
        {
            InitializeComponent();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DeliveryStatus Status = new DeliveryStatus();
            Status.StartPosition = FormStartPosition.Manual;
            Status.Left = 10;
            Status.Top = 20;
            this.Hide();
            Status.Show();

            
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddDelivery add = new AddDelivery();
            add.StartPosition = FormStartPosition.Manual;
            add.Left = 10;
            add.Top = 20;
            add.Show();
            
        }

        private void Delivery_Load(object sender, EventArgs e)
        {

        }
    }
}
