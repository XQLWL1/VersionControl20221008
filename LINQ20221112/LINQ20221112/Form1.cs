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
        List<Ramen> ramens = new List<Ramen>();
        List<Brand> brands = new List<Brand>();

        public Form1()
        {
            InitializeComponent();
            LoadData("ramen.csv");
            GetCountries();
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
                string marka = sor[0];
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

                    //countries.Add(eredmeny);
                    Country currentCountry = AddCountry(country); //aktuális ország
                    Brand currentmarka = AddBrand(marka);

                    Ramen ramen = new Ramen
                    {
                        ID = ramens.Count+1,
                        CountryFK = currentCountry.ID,
                        Country = currentCountry,
                        Stars = Convert.ToDouble(sor[3]),
                        Name = currentCountry.Name,
                        Brand = currentmarka

                    };
                    ramens.Add(ramen);
                }
            }

            streamReader.Close();

            /*using (resource)
            {

            }*/


        }

        Country AddCountry(string country)
        {
            var eredmeny = (from c in countries
                            where c.Name.Equals(country)
                            select c).FirstOrDefault();

            if (eredmeny == null) //nincs ilyen ország a listában, újat létrehozunk
            {
                eredmeny = new Country
                {
                    ID = countries.Count,
                    Name = country
                };

                countries.Add(eredmeny);
            }
            return eredmeny; 
        }

        Brand AddBrand(string marka)
        {
            var eredmeny = (from c in brands
                            where c.Name.Equals(brands)
                            select c).FirstOrDefault();

            if (eredmeny == null) //nincs ilyen ország a listában, újat létrehozunk
            {
                eredmeny = new Brand
                {
                    ID = brands.Count,
                    Name = marka
                };

                brands.Add(eredmeny);
            }
            return eredmeny;
        }

        void GetCountries()
        {
            var countriesList = from c in countries
                                where c.Name.Contains(txtCountryFilter.Text)
                                orderby c.Name
                                select c;
            listCountries.DataSource = countriesList.ToList();
            listCountries.DisplayMember = "Name";
        }

        private void txtCountryFilter_TextChanged(object sender, EventArgs e)
        {
            GetCountries();
        }
    }
}
