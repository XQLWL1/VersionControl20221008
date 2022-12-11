using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ValueAtRisk_VAR
{
    public partial class Form1 : Form
    {

        PortfolioEntities context = new PortfolioEntities(); //context példányosítása
        List<Tick> ticks;

        public Form1()
        {
            InitializeComponent();

            ticks =context.Ticks.ToList();
            dataGridView1.DataSource=ticks;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
