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
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Image image; 
            if (player == 1) 
            {
                player = 2;
                // Обращаемся к нашим ресурсам
                image = Properties.Resources.Крестик;
            }
            else
            {
                player = 1;
                // Обращаемся к нашим ресурсам
                image = Properties.Resources.Нолик;
            }
            
            pictureBox1.Image = image;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Image image;
            if (player == 1)
            {
                player = 2;
                // Обращаемся к нашим ресурсам
                image = Properties.Resources.Крестик;
            }
            else
            {
                player = 1;
                // Обращаемся к нашим ресурсам
                image = Properties.Resources.Нолик;
            }


            pictureBox2.Image = image;
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Image image;
            if (player == 1)
            {
                player = 2;
                // Обращаемся к нашим ресурсам
                image = Properties.Resources.Крестик;
            }
            else
            {
                player = 1;
                // Обращаемся к нашим ресурсам
                image = Properties.Resources.Нолик;
            }

        
            pictureBox9.Image = image;
        }
    }
}
