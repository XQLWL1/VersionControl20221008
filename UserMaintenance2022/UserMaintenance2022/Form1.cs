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
            btnDelete.Text = Resource1.Delete; //button3


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
            //Fájlba írás, én adom meg a fájl típusát pl.: txt:
            /*SaveFileDialog saveFileDialog = new SaveFileDialog();
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

            }*/



            //CSV-be mentem ki a fájlt:
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = @"C:\Users\vivi22\Source\Repos\2022 ősz IRF\VersionControl20221008\UserMaintenance2022\UserMaintenance2022\bin\Debug";

            // A kiválasztható fájlformátumokat adjuk meg ezzel a sorral. Jelen esetben csak a csv-t fogadjuk el
            saveFileDialog.Filter = "Comma Seperated Values (*.csv)|*.csv";

            // A csv lesz az alapértelmezetten kiválasztott kiterjesztés
            saveFileDialog.DefaultExt = "csv";

            // Ha ez igaz, akkor hozzáírja a megadott fájlnévhez a kiválasztott kiterjesztést, de érzékeli, ha a felhasználó azt is beírta és nem fogja duplán hozzáírni
            saveFileDialog.AddExtension = true;

            //a fájl nevét adom meg
            saveFileDialog.FileName = "userList";

            saveFileDialog.ShowDialog();

            /*A StreamWriter paraméterei:
             * 1) Fájlnév: mi itt azt a fájlnevet adjuk át, amit a felhasználó az sfd dialógusban megadott
             * 2) Append: ha igaz és már létezik ilyen fájl, akkor a végéhez fűzi a sorokat, ha hamis, akkor felülírja a létező fájlt
             * 3) Karakterkódolás: a magyar nyelvnek is megfelelő legáltalánosabb karakterkódolás az UTF8*/
            using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8))
            {
                //végig megyek a lista elemein, mert a StreamWriter részenként építi fel a sorokat.
                foreach (var u1 in users)
                {
                    sw.Write(u1.ID);
                    sw.Write(";");
                    sw.Write(u1.FullName);

                    sw.WriteLine();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            User választottUser = (User)listBox1.SelectedItem;
            users.Remove(választottUser);
        }
    }
}
