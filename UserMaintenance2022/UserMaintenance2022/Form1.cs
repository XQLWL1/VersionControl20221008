using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserMaintenance2022.Entities;

namespace UserMaintenance2022
{
    public partial class Form1 : Form
    {

        BindingList<User> users = new BindingList<User>();

        public Form1()
        {
            InitializeComponent();

            label1.Text = Resource1.FullName; //label1
            btnAdd.Text = Resource1.Add; // button1
            btnFajl.Text = Resource1.FajlbaIras; //button2


            //Lista adaforrása:
            listBox1.DataSource = users; //lista a users mappát jeleníti meg
            listBox1.ValueMember = "ID"; //ID-val dolgozik, de
            listBox1.DisplayMember = "FullName"; //a nevet jeleníti meg

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            User user = new User();

            user.FullName = textBox3.Text;

            users.Add(user);
            
        }

        private void btnFajl_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog()==DialogResult.OK)
            {
                //MessageBox.Show("próba mentés");

                using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName))
                {
                    foreach (User item in users)
                    {
                        streamWriter.WriteLine(item.ID + ";" + item.FullName);
                    }
                }
            }
        }
    }
}
