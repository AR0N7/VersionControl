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
using UserMaintenance.Entities;

namespace UserMaintenance
{
    public partial class Form1 : Form
    {
        BindingList<User> users = new BindingList<User>();

        public string FullName { get; private set; }

        public Form1()
        {
            InitializeComponent();
            label1.Text = Resource.FullName;
            //label2.Text = Resource.FirstName;
            button1.Text = Resource.Add;
            button2.Text = Resource.B2;
            button3.Text = Resource.Delete;

            listBox1.DataSource = users;
            listBox1.ValueMember = "ID";
            listBox1.DisplayMember = "FullName";

          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();

            save.FileName = ".txt";

            save.Filter = "Text File | *.txt";

            if (save.ShowDialog() == DialogResult.OK)

            {

                StreamWriter writer = new StreamWriter(save.OpenFile());

                for (int i = 0; i < listBox1.Items.Count; i++)

                {

                    writer.WriteLine(listBox1.Items[i].ToString());

                }

                writer.Dispose();

                writer.Close();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var u = new User();
            {
                FullName = textBox1.Text;
                //FirstName = textBox2.Text;
            }

            users.Add(u);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var rem = listBox1.SelectedItem;
            User torolni = (from x in users
                      where x == rem
                      select x).FirstOrDefault();
            users.Remove(torolni);
        }
    }
}
