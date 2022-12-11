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

        public Form1()
        {
            InitializeComponent();

            ticks =context.Ticks.ToList();
            dataGridView1.DataSource=ticks;

            CreatePortfolio();
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
    }
}
