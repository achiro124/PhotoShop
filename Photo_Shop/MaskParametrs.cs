using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Photo_Shop
{
    public partial class MaskParametrs : Form
    {
        private PictureBox pictureBox;
        private Image img;
        public MaskParametrs(PictureBox pictureBox, Image img)
        {
            InitializeComponent();
            listBox1.SelectedIndex = 0;
            this.pictureBox = pictureBox;
            this.img = img;
            //img = new Image((Bitmap)pictureBox.Image.Clone());

            textBox1.TextChanged += new EventHandler(Change_value);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (listBox1.SelectedIndex)
            {
                case 0:
                    groupBox1.Visible = false;
                    groupBox2.Visible = false;
                    groupBox3.Visible = true;
                    groupBox3.Location = new Point(groupBox3.Location.X, 40);
                    break;
                case 1:
                    groupBox1.Visible = true;
                    groupBox2.Visible = false;
                    groupBox3.Visible = false;
                    groupBox3.Location = new Point(groupBox3.Location.X, 80);
                    break;
                case 2:
                    groupBox1.Visible = true;
                    groupBox2.Visible = true;
                    groupBox3.Visible = false;
                    groupBox3.Location = new Point(groupBox3.Location.X, 80);
                    break;
            }
        }

        private void  Change_value(object sender, EventArgs e)
        {
            switch (listBox1.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    Image img1 = img.ChangeClarity(50);
                    pictureBox.Image = (Bitmap)img1.Img.Clone();
                    break;
                case 2:
                    break;

            }
        }
    }
}
