using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ValueAtRisk_VAR.Entities;

namespace ValueAtRisk_VAR
{
    public partial class Form1 : Form
    {

        PortfolioEntities context = new PortfolioEntities(); //context példányosítása
        List<Tick> ticks;

        List<PortfolioItem> portfolios = new List<PortfolioItem>();

        List<decimal> Nyereségek;

        public Form1()
        {
            InitializeComponent();

            ticks =context.Ticks.ToList();
            dataGridView1.DataSource=ticks;

            CreatePortfolio();

            Nyereseg();
        }

        private void Nyereseg()
        {
            Nyereségek = new List<decimal>();

            //intervallum beállítása, ami 30 --> ablak mérete
            int intervalum = 30;

            //kezdő dátum: kereskedés kezdete
            DateTime kezdőDátum = (from x in ticks select x.TradingDay).Min();

            //záródátum: 2016.12.30
            DateTime záróDátum = new DateTime(2016, 12, 30);

            //két dátum különbségére használható:
            TimeSpan z = záróDátum - kezdőDátum;

            //számláló ciklus. A napok különbsége mínusz az intervallum
            //lekéri a portófiló értékét: 0-val kezdünk, ezért az 30 lesz, majd vesszük az 1. nap értékét és
            //a kettőt kivonjuk egymásból, majd ezt hozzáadjuk a nyereségekhez.
            //addig csináljuk, amíg el nem érjük a 2016.12.30-at.
            for (int i = 0; i < z.Days - intervalum; i++)
            {
                decimal ny = GetPortfolioValue(kezdőDátum.AddDays(i + intervalum))
                           - GetPortfolioValue(kezdőDátum.AddDays(i));
                Nyereségek.Add(ny);

                //ezt kiírjuk:
                Console.WriteLine(i + " " + ny);
            }

            ////Lesz egy lista, mely a nyereségeket mutatja meg az adott napokon és rendezi
            var nyereségekRendezve = (from x in Nyereségek
                                      orderby x
                                      select x)
                                        .ToList();

            //A listás értéket 5-el elosztja. Ez lesz a VAR (value at risk) érték
            MessageBox.Show(nyereségekRendezve[nyereségekRendezve.Count() / 5].ToString());
        }

        private void CreatePortfolio()
        {
            //throw new NotImplementedException();

            PortfolioItem portfolioItem = new PortfolioItem();
            portfolioItem.Index = "OTP";
            portfolioItem.Volume = 10;
            portfolios.Add(portfolioItem);

            PortfolioItem portfolioItem2 = new PortfolioItem();
            portfolioItem2.Index = "ZWACK";
            portfolioItem2.Volume = 10;
            portfolios.Add(portfolioItem2);

            PortfolioItem portfolioItem3 = new PortfolioItem();
            portfolioItem3.Index = "ELMU";
            portfolioItem3.Volume = 10;
            portfolios.Add(portfolioItem2);

            dataGridView2.DataSource= portfolios;
        }

        private decimal GetPortfolioValue(DateTime date)
        {
            decimal value = 0;

            //foreach ciklus végig megy a portfolio elemein (elemek: OTP,ZWACK, ELMU).
            foreach (var item in portfolios)
            {
                var last = (from x in ticks //ticks listában keres
                            where item.Index == x.Index.Trim()
                               && date <= x.TradingDay //dátumnak kisebbnek vagy =-nek kell lennie a Trading Day-el
                            select x)
                            .First(); //az első elem kell

                //az értékhez hozzáadja az utolsó elem értékét és azt megszorozza a Volume-val.
                value += (decimal)last.Price * item.Volume; 
            }
            return value;
        }
    }
}
