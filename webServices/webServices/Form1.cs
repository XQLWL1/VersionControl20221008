using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using webServices.Entities;

namespace webServices
{
    public partial class Form1 : Form
    {
        BindingList<RateData> rates = new BindingList<RateData>();

        public Form1()
        {
            InitializeComponent();

            var mnbService = new webServices.MnbServiceReference.MNBArfolyamServiceSoapClient();

            var request = new webServices.MnbServiceReference.GetExchangeRatesRequestBody()
            {
                currencyNames = "EUR",
                startDate="2020-01-01",
                endDate="2020-06-30"
            };

            var response = mnbService.GetExchangeRates(request);
            var result = response.GetExchangeRatesResult;


            XML();
            
        }

        public void XML(string resultstring)
        {
            var xml = new XmlDocument();
            xml.LoadXml(resultstring);

            foreach (XmlElement item in xml.DocumentElement)
            {
                //Adatsor létrehozása:
                RateData rateData = new RateData();

                //Dátum:
                rateData.Date = DateTime.Parse(item.GetAttribute("date"));

                //Valuta:
                XmlElement childElement = (XmlElement)item.ChildNodes[0];

                if (childElement != null)
                {
                    rateData.Currency = childElement.GetAttribute("currency");

                    //Az alapegység a gyermek elem "unit" attribútumának segítségével határozható meg
                    decimal unit = decimal.Parse(childElement.GetAttribute("unit"));

                    //az egységhez tartozó érték pedig a gyermek elem InnerText tulajdonságából kapható meg
                    rateData.Value = decimal.Parse(childElement.InnerText);

                    if (unit!=0)
                    {
                        rateData.Value = rateData.Value / unit;
                    }

                    //hozzáadjuk a listához
                    rates.Add(rateData);
                }


            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.DataSource= rates;
        }

        
    }
}
