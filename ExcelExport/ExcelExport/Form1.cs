using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelExport
{
    public partial class Form1 : Form
    {
        List<Flat> Flats = new List<Flat>();
        RealEstateEntities context=new RealEstateEntities ();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void LoadData()
        {
            Flats = context.Flats.ToList();
        }
    }
}
