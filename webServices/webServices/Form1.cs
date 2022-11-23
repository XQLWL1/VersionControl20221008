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
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using webServices.Entities;
using webServices.MnbServiceReference;

namespace webServices
{
    public partial class Form1 : Form
    {
        BindingList<RateData> rates = new BindingList<RateData>();
        BindingList<string> currencies = new BindingList<string>();

        public Form1()
        {
            InitializeComponent();

            //egybe kell szervezni az alábbi függvényeket:
            //XML(getRates());

            //chartData();

            /*Ehhez kijelölöm a 3 függvényt, jobb klikk, Qick actions and refactorings, extract method,
            elnevezem az új függvényt, 
            apply*/

            currencyXml(getCurrencies());

            RefreshData();

        }

        private void RefreshData()
        {
            //ürítsd le a Rates lista tartalmát
            rates.Clear();

            //XML meghívása:
            XML(getRates());

            //dataGridView adatforrás beállítása:
            dataGridView1.DataSource = rates;

            //valuta adatforrásának beállítása:
            comboBox1.DataSource = currencies;

            chartData();

        }

        private void chartData()
        {
            chartRateData.DataSource = rates;

            //A tömb első elemét érdemes lekérdezni egy változóba,
            //hogy könnyebb legyen átírni a tulajdonságait.
            var series = chartRateData.Series[0];

            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
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

        public void XML(string resultstring)
        {
            XmlDocument xml = new XmlDocument();
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

                    if (unit != 0)
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
            currencies.Clear();

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlstring);

            foreach (XmlElement item in xml.DocumentElement.ChildNodes[0])
            {
                string s = item.InnerText;
                currencies.Add(s);
            }
        }

        private string getCurrencies()
        {
            var mnbService = new webServices.MnbServiceReference.MNBArfolyamServiceSoapClient();

            GetCurrenciesRequestBody req = new GetCurrenciesRequestBody();
            var response1 = mnbService.GetCurrencies(req);

            string result = response1.GetCurrenciesResult;

            File.WriteAllText("currency.xml", result);

            return response1.GetCurrenciesResult;

        }

        private string getRates()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();

            GetExchangeRatesRequestBody req = new GetExchangeRatesRequestBody();

            /*Át kell alakítani az alábbi 3 sort, mivel az adatokat a form1-re rakott felületről kell vennie
             * req.currencyNames = "EUR";
            req.startDate = "2020-01-01";
            req.endDate = "2020-6-30";*/

            req.currencyNames = (string)comboBox1.SelectedItem;
            req.startDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            req.endDate = dateTimePicker2.Value.ToString("yyyy-MM-dd");

            var response = mnbService.GetExchangeRates(req);
            //var result = response.GetExchangeRatesResult;
            return response.GetExchangeRatesResult;

            //File.WriteAllText("text.xml", result);
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
