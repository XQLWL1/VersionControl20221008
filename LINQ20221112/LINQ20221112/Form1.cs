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

namespace LINQ20221112
{
    public partial class Form1 : Form
    {

        List<Country> countries = new List<Country>(); 

        public Form1()
        {
            InitializeComponent();
            LoadData("ramen.csv");
        }

        void LoadData(string fileName)
        {
            //országok a 3. oszlop volt
            //egy ország többször előfordulhat, csak egyszer kell betenni a listába

            StreamReader streamReader = new StreamReader(fileName);
            streamReader.ReadLine(); //átugrik az első soron

            while (!streamReader.EndOfStream)
            {
                string[] sor = streamReader.ReadLine().Split(';'); //split felbontja tömbre ";" jel alapján
                string country = sor[2];
                //var eredmeny = countries.Where(i => i.Name.Equals(country)).FirstOrDefault(); //LINQ

                //LINQ SQL-es:
                var eredmeny = (from c in countries 
                               where c.Name.Equals(country) 
                               select c).FirstOrDefault();

                if (eredmeny==null) //nincs ilyen ország a listában, újat létrehozunk
                {
                    eredmeny = new Country
                    {
                        ID = countries.Count,
                        Name = country
                    };

                    countries.Add(eredmeny);
                }
            }

            streamReader.Close();

            /*using (resource)
            {

            }*/


        }
    }
}
