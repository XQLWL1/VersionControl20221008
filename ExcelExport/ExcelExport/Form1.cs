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
            /*try and catch használata
             hibakezelő
            mivel beragadhat az excel, ezért kell a hibakezelés, */

            try
            {
                xlApp = new Excel.Application(); //új excel létrehozás
                xlWB = xlApp.Workbooks.Add(); //az excelen belüli workbook és sheet kiválasztása - hova írhatok adatot
                xlSheet = xlWB.ActiveSheet;

                CreateTable();

                xlApp.Visible = true; //felhasználó számára elérhetővétesszük az alkalmazást:
                xlApp.UserControl = true; //felhasználó általi vezérlés engedélyezése

            }
            catch (Exception exception) 
            //éppen aktuális hibához juthatunk hozzá, ha példányként hozzáférhetővé tesszük az exception objektumot
            {
                string errorMessage=exception.Message;
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //hibás excelek bezárása
                xlWB.Close(false, Type.Missing, Type.Missing);
                xlApp.Quit();
                xlWB= null;
                xlApp= null;
            }
            

        }

        private void CreateTable()
        {
            string[] fejlec = new string[]
            {
                "Kód",
                "Eladó",
                "Oldal",
                "Kerület",
                "Lift",
                "Szobák száma",
                "Alapterület (m2)",
                "Ár (mFt)",
                "Négyzetméterár (Ft/m2)"
            };

            xlSheet.Cells[1, 1] = "Teszt"; //Teszt szó kiírása próbaképpen

            for (int i = 1; i <= fejlec.Length; i++)
            {
                xlSheet.Cells[1, i] = fejlec[i - 1]; //fejléc egymás mellé kerül
            }

            //Két dimenziós tömb létrehozása az adatok tárolására.
            //sor és oszlop száma lesz a mérete a táblázatnak
            //nem csak szöveg, hanem szám típusú változók is vannak, ezért object tömbünk van
            object[,] values = new object[Flats.Count, fejlec.Length];

            //values nevű tömb feltöltése az adatokkal
            int counter = 0;

            foreach (Flat item in Flats)
            {
                values[counter, 0] = item.Code;
                values[counter, 1] = item.Vendor;
                values[counter, 2] = item.Side;
                values[counter, 3] = item.District;
                values[counter, 4] = item.Elevator;
                values[counter, 5] = item.NumberOfRooms;
                values[counter, 6] = item.FloorArea;
                values[counter, 7] = item.Price;

                //values[counter, 8] = ""; 
                //négyzetméter árat kalkuláció alapján kell majd kiszámolni, ezért üres
                //Négyzetméter kiszámolása: Ár * 1.000.000/alapterület ->
                //(szorzás azért kell, mert az Ár az MFt, a négyzetméternél meg Ft kell)

                values[counter, 8] = "=" + GetCell(counter+2, 8); //első sor a fejléc, ezért counter+2

                counter++;
            }

            //A segédfüggvény felhasználásával írd ki a values tömb tartalmát az Excel fájlba.
            //get_Range: az excel osztály nem hozza a hivatkozást,
            //paramétereit se ismeri. Bal felső és jobb alsó sarkát adja meg.
            
            xlSheet.get_Range
                (
                GetCell(2, 1), //2,1-ből indulunk
                GetCell(1 + values.GetLength(0), values.GetLength(1)) //values.GetLength(0): lakásoknak és az oszlopfejléceknek a db száma
                ).Value2 = values; //Value2 az az amibe az értéket bele kell írni

        }


        void LoadData()
        {
            Flats = context.Flats.ToList(); // flat elemekből álló lista bemásolása
        }

        private string GetCell(int x, int y) //x és y koorniták
        {
            string ExcelCoordinate = "";
            int dividend = y; //y koordinátával kezd, amit egyre jobban csökkenteni fog
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26; //angol ABC karakterszáma a 26-os

                //65 maradékánál fogja összetenni, hogy milyen betűnél járunk
                //65: a nagy A betű az ASCII tábla szerint
                ExcelCoordinate = Convert.ToChar(65 + modulo).ToString() + ExcelCoordinate; 

                dividend = (int)((dividend - modulo) / 26);
            }

            ExcelCoordinate += x.ToString();

            return ExcelCoordinate;
        }
    }
}
