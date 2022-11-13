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

        Excel.Application xlApp; // Ez az Excel alkalmazás
        Excel.Workbook xlWB; // létrehoz workbook-ot
        Excel.Worksheet xlSheet; // sheet-et hoz létre


        public Form1()
        {
            InitializeComponent();
            LoadData();

            CreateExcel();
        }

        private void CreateExcel()
        {
            //throw new NotImplementedException();

            xlApp = new Excel.Application (); //új excel létrehozás
            xlWB = xlApp.Workbooks.Add (); //az excelen belüli workbook és sheet kiválasztása - hova írhatok adatot
            xlSheet = xlWB.ActiveSheet;

            //CreateTable();

            xlApp.Visible= true; //felhasználó számára elérhetővétesszük az alkalmazást:
            xlApp.UserControl = true; //felhasználó általi vezérlés engedélyezése

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
