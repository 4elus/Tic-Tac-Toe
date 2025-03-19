using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tic_Tac_Toe
{

    public partial class Form1 : Form
    {
        int player = 1;
        int loc_x = 120; 
        int loc_y = 50;
        public Form1()
        {
            //PictureBox[] collectionPicturteBoxes = { 
            //new PictureBox(),
            //new PictureBox()
            //};
            //int a =  0;
            //int[] abc = {};
            InitializeComponent();
            int c = 1;
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    PictureBox picture = new PictureBox();
                    picture.Left = loc_x;
                    picture.Top = loc_y;
                    picture.Width = 104;
                    picture.Height = 104;

                    Image image = Properties.Resources.template;
                    picture.Image = image;
                    picture.Name = "PictureBox" + c;
                    picture.Click += PictureBoxClick;
                    picture.SizeMode = PictureBoxSizeMode.StretchImage;
                    this.Controls.Add(picture);
                    loc_x += 121;
                    c += 1;
                }
                loc_y += 114;
                loc_x = 120;
            }
        }

        private void PictureBoxClick(object sender, EventArgs e)
        {
            PictureBox picture = sender as PictureBox;
            Image image = Properties.Resources.Крестик;
            picture.Image = image;
        }

    }
}