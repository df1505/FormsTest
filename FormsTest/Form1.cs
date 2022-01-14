using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace FormsTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            LoadPictureBox2();

#if (BOGUS)
            string filein = "c:\\Users\\David\\temp\\foo.jpg";
            string fileout = "c:\\Users\\David\\temp\\foo1.jpg";

            clsReadMetaData rdr = new clsReadMetaData();

            string comment = "Test comment";

            //rdr.SetComment(filein, comment, fileout);

            rdr.SetComment(fileout, comment);

            string cmnt = rdr.GetComment(fileout);

            int foo = 1;
#endif
        }


        void LoadPictureBox2()
        {
            string filein = "c:\\Users\\DaveFindley\\temp\\test.jpg";
            FileStream fs = new FileStream(filein, FileMode.Open, FileAccess.Read);
            //Image image = Image.FromStream(fs);
            //pictureBox2.Image = image;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string comment = richTextBox1.Text;
            clsReadMetaData rdr = new clsReadMetaData();
            string fileout = "c:\\Users\\DaveFindley\\temp\\foo1.jpg";
            rdr.SetComment(fileout, comment);
            richTextBox2.Text = rdr.GetComment(fileout);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string folderName;
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderName = folderBrowserDialog1.SelectedPath;
                textBoxOut.Text = folderName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox1.Items.Add("itemthatiswaytoolong");
            listBox1.Items.Add("item2");
            listBox1.Items.Add("item3");
            listBox1.Items.Add("item4");
            listBox1.Items.Add("item5");
            listBox1.Items.Add("item6");
            listBox1.Items.Add("item7");
            listBox1.Items.Add("item8");
            listBox1.Items.Add("item9");
            listBox1.Items.Add("item10");
            listBox1.Items.Add("item10");
            listBox1.Items.Add("item11");
            listBox1.Items.Add("item12");

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Load("c:\\Users\\David\\Temp\\foo.jpg");


        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
             textBoxOut.Text = listBox1.SelectedItem.ToString();

             
        }

        private void Exp_Click(object sender, EventArgs e)
        {
            string filename = "Autofill.txt";
            Process.Start("notepad.exe", filename);
        }
    }
}
