using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using webServices.Entities;

namespace webServices
{
    public partial class Form1 : Form
    {
        BindingList<RateData> rates = new BindingList<RateData>();
        private string resultstring;
        BindingList<string> currencies = new BindingList<string>();

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


            XML(resultstring);
            resultstring = 

            chartData();

            currencyXml(getCurrenies())

            RefreshData();

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

        //Az 5. feladatban leírtak alapján a visszakapott XML-ből töltsd fel a Currencies listát
        private void currencyXml(string xmlstring)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlstring);

            foreach (XmlElement item in xml.DocumentElement.ChildNodes[0])
            {
                string s = item.InnerText;
                currencies.Add(s);
            }
        }
        

        private void chartData()
        {
            chartRateData.DataSource = rates;

            Series series = chartRateData.Series[0];
            series.ChartType = SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";
            series.BorderWidth = 2; //Az adatsor vastagsága legyen kétszeres

            //Ne látszódjon oldalt a címke (legend)
            var legend = chartRateData.Legends[0];
            legend.Enabled = false;

            //Ne látszódjanak a fő grid vonalak se az X, se az Y tengelyen
            var chartArea = chartRateData.ChartAreas[0];
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;

            //Az Y tengely ne nullától induljon(ez egy bool tulajdonság)
            chartArea.AxisY.IsStartedFromZero = false;
        }

        private void RefreshData()
        {
            //ürítsd le a Rates lista tartalmát
            rates.Clear();

            //valuta adatforrásának beállítása:
            comboBox1.DataSource = currencies;

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.DataSource = rates;

            //A tömb első elemét érdemes lekérdezni egy változóba,
            //hogy könnyebb legyen átírni a tulajdonságait.
            var series = chartRateData.Series[0];

            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
