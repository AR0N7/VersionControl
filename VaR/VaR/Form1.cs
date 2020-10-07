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
using VaR.Entities;

namespace VaR
{
    public partial class Form1 : Form
    {
        PortfolioEntities context = new PortfolioEntities();
        List<Tick> Ticks;
        List<decimal> Nyereségek = new List<decimal>();
        List<PortfolioItem> Portfolio = new List<PortfolioItem>();

        public Form1()
        {
            InitializeComponent();
            Ticks = context.Ticks.ToList();
            dataGridView1.DataSource = Ticks;
            CreatePortfolio();

            
            int intervalum = 30;
            DateTime kezdőDátum = (from x in Ticks select x.TradingDay).Min();
            DateTime záróDátum = new DateTime(2016, 12, 30);
            TimeSpan z = záróDátum - kezdőDátum;
            for (int i = 0; i < z.Days - intervalum; i++)
            {
                decimal ny = GetPortfolioValue(kezdőDátum.AddDays(i + intervalum))
                           - GetPortfolioValue(kezdőDátum.AddDays(i));
                Nyereségek.Add(ny);
                Console.WriteLine(i + " " + ny);
            }

            var nyereségekRendezve = (from x in Nyereségek
                                      orderby x
                                      select x)
                                        .ToList();
            MessageBox.Show(nyereségekRendezve[nyereségekRendezve.Count() / 5].ToString());
        }

        private void CreatePortfolio()
        {
            Portfolio.Add(new PortfolioItem() { Index = "OTP", Volume = 10 });
            Portfolio.Add(new PortfolioItem() { Index = "ZWACK", Volume = 10 });
            Portfolio.Add(new PortfolioItem() { Index = "ELMU", Volume = 10 });

            dataGridView2.DataSource = Portfolio;
        }

        private decimal GetPortfolioValue(DateTime date)
        {
            decimal value = 0;
            foreach (var item in Portfolio)
            {
                var last = (from x in Ticks
                            where item.Index == x.Index.Trim()
                               && date <= x.TradingDay
                            select x)
                            .First();
                value += (decimal)last.Price * item.Volume;
            }
            return value;
        }

            

        private void button1_Click(object sender, EventArgs e)
        {
            //SaveFileDialog save = new SaveFileDialog();

            //save.FileName = ".txt";

            //save.Filter = "Text File | *.txt";

            //if (save.ShowDialog() == DialogResult.OK)

            //{
                
                //using (StreamWriter sw = new StreamWriter(save.FileName, false, Encoding.UTF8))
                //{
                    var nyereségekRendezve = (from x in Nyereségek
                                              orderby x
                                              select x)
                                        .ToList();

                    int i = 0;

                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.DefaultExt = "txt";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        Stream FileST = sfd.OpenFile();
                        StreamWriter sw = new StreamWriter(FileST);

                        sw.WriteLine("Időszak, Nyereség");
                        foreach (var x in nyereségekRendezve)
                        {
                       
                            sw.WriteLine(i+"; "+x);
                            i++;
                        }

                         sw.Close();
                         FileST.Close();

                    }
        }
    
    }
}
