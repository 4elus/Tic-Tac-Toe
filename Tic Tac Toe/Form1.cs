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
        int[,] field = { 
            { 0, 0, 0 },
            { 0, 0, 0 },
            { 0, 0, 0 },
        };
        public Form1()
        {
            //PictureBox[] collectionPicturteBoxes = { 
            //new PictureBox(),
            //new PictureBox()
            //};
            //int a =  0;
            //int[] abc = {};
            InitializeComponent();
            
            //int c = 1;
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    PictureBox picture = new PictureBox();
                    picture.Left = loc_x;
                    picture.Top = loc_y;
                    picture.Width = 104;
                    picture.Height = 104;

                    Image image = Properties.Resources.template;
                    picture.Image = image;
                    picture.Name = "PictureBox" + i.ToString() + j.ToString();
                    picture.Tag = "empty";
                    picture.Click += PictureBoxClick;
                    picture.SizeMode = PictureBoxSizeMode.StretchImage;
                    this.Controls.Add(picture);
                    loc_x += 121;
                 
                }
                loc_y += 114;
                loc_x = 120;
            }
        }

        private void PictureBoxClick(object sender, EventArgs e)
        {
            PictureBox picture = sender as PictureBox;

            if (picture.Tag.ToString() != "empty")
            {
                return;
            }

            // В зависимости от игрока выбираем картинку: крестик или нолик
            Image image = player == 1 ? Properties.Resources.Крестик : Properties.Resources.Нолик;
            picture.Image = image;
            int row = Convert.ToInt32(picture.Name[10].ToString());
            int col = Convert.ToInt32(picture.Name[11].ToString());

           // MessageBox.Show(row + ", " + col);
            if (player == 1)
            {
                picture.Tag = "X";
                field[row, col] = 1;
            }
            else
            {
                picture.Tag = "O";
                field[row, col] = 2;
            }

            //picture.Tag = player == 1 ? "X" : "O";

            player = (player == 1) ? 2 : 1;

            check_win();
        }

        private void check_win()
        {
            //if (field[0, 0] == 1 && field[0, 1] == 1 && field[0, 2] == 1) {
            int sum = 0;
            for (int i = 0; i <= field.GetUpperBound(0); i++)
            {
                //sum = 0;
                for (int j = 0; j <= field.GetUpperBound(0); j++)
                {
                    if (field[i, j] == 1)
                    {
                        sum += 1;
                    }
                }

            }
            
            if (sum == 3)
            {
                MessageBox.Show("Победа!");
            }
        }
    }
}