using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection; //technikai könyvtár, excel hiányzó adatait ezzel lehet kezelni, mert ebben van olyan érték, mely az excelnek megfelel

namespace ExcelExport
{
    public partial class Form1 : Form
    {
        List<Flat> Flats = new List<Flat>();
        RealEstateEntities context=new RealEstateEntities ();

        public Form1()
        {
            InitializeComponent();
            LoadData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void LoadData()
        {
            Flats = context.Flats.ToList(); // flat elemekből álló lista bemásolása
        }
    }
}
