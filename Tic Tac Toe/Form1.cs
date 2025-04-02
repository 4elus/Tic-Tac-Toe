using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tic_Tac_Toe
{

    public partial class Form1 : Form
    {
        // Текущий игрок: 1 - крестики, 2 - нолики
        int player = 1;
        Random random = new Random();
        int loc_x = 120;
        int loc_y = 50;

        // Флаг окончания игры
        bool gameOver = false;

        // Игровое поле: 0 - пусто, 1 - крестик, 2 - нолик
        int[,] field = new int[3, 3];

        // Лейбл для вывода результата игры
        Label resultLabel;

        // Кнопка рестарта
        Button restartButton;
        int[,] win_stack = new int[3, 3];
        PictureBox[,] pictures = new PictureBox[3, 3];
        bool chance_win = false; // шанс выигрыша
        string position = "";
        int count = 0;
        public Form1()
        {
            InitializeComponent();
            CreateGameField();
            CreateResultLabel();
            CreateRestartButton();
        }

        private void CreateGameField()
        {
            // Создание игрового поля из PictureBox
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    PictureBox picture = new PictureBox();
                    picture.Left = loc_x;
                    picture.Top = loc_y;
                    picture.Width = 114;
                    picture.Height = 104;
                    picture.Image = Properties.Resources.template;
                    picture.Name = "PictureBox" + i.ToString() + j.ToString();
                    picture.Tag = "empty";
                    picture.Click += PictureBoxClick;
                    picture.SizeMode = PictureBoxSizeMode.Zoom;
                    pictures[i, j] = picture;
                    this.Controls.Add(pictures[i, j]);
                   
                    loc_x += 121;
                }
                loc_y += 114;
                loc_x = 120;
            }
        }

        private void CreateResultLabel()
        {
            // Лейбл для вывода сообщения о результате
            resultLabel = new Label();
            resultLabel.Name = "ResultLabel";
            resultLabel.Text = ""; // изначально пусто
            resultLabel.Font = new Font("Arial", 16, FontStyle.Bold);
            resultLabel.Location = new Point(120, 10);
            resultLabel.AutoSize = true;
            this.Controls.Add(resultLabel);
        }

        private void CreateRestartButton()
        {
            // Кнопка для рестарта игры
            restartButton = new Button();
            restartButton.Name = "RestartButton";
            restartButton.Text = "Рестарт";
            restartButton.Font = new Font("Arial", 12);
            restartButton.Location = new Point(350, 10);
            restartButton.AutoSize = true;
            restartButton.Click += RestartButton_Click;
            // По умолчанию кнопку скрываем
            restartButton.Visible = false;
            this.Controls.Add(restartButton);
        }

        private void PictureBoxClick(object sender, EventArgs e)
        {
            // Если игра окончена, ходы не обрабатываем
            if (gameOver) return;

            //// Ход компьютера
            //if (player == 2)
            //{
            //    int row_random = random.Next(0, 3);
            //    int col_random = random.Next(0, 3);

            //    MessageBox.Show(row_random + " : " + col_random);
            //}


            PictureBox picture = sender as PictureBox;


           

            

            if (picture.Tag.ToString() != "empty")
                return;

            // Устанавливаем картинку в зависимости от игрока
            picture.Image = player == 1 ? Properties.Resources.Крестик : Properties.Resources.Нолик;
            int row = Convert.ToInt32(picture.Name[10].ToString());
            int col = Convert.ToInt32(picture.Name[11].ToString());

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

            // Проверка победы или ничьей
            int winner = CheckWin();
            if (winner != 0)
            {
                resultLabel.Text = $"Победил {(winner == 1 ? "крестик" : "нолик")}!";
                PaintWinSet();
                EndGame();
                return;
            }
            else if (IsBoardFull())
            {
                resultLabel.Text = "Ничья!";
                EndGame();
                
                return;
            }

            // Смена игрока
            player = (player == 1) ? 2 : 1;

            Move_PC();
        }

        private void Move_PC()
        {

            int row_random = 0;
            int col_random = 0;
            bool key = true; // true проверка осуществляется, false  проверка прекращается
            // Если клетка свободна
            while (key == true)
            {
                chance_win = PreventWin();

                if (chance_win == true)
                {
                    row_random = Convert.ToInt32(position[0].ToString());
                    col_random = Convert.ToInt32(position[1].ToString());
                    MessageBox.Show(row_random + " : " + col_random);
                }
                else
                {
                  
                    row_random = random.Next(1, 3);
                    col_random = random.Next(1, 3);
                    
                }

                if (field[row_random, col_random] == 0)
                {
                    MessageBox.Show(row_random + " : " + col_random);
                    field[row_random, col_random] = 2;
                    pictures[row_random, col_random].Tag = "O";
                    pictures[row_random, col_random].Image = player == 1 ? Properties.Resources.Крестик : Properties.Resources.Нолик;
                    // Смена игрока
                    player = (player == 1) ? 2 : 1;
                    key = false;



                    // Проверка победы или ничьей
                    int winner = CheckWin();
                    if (winner != 0)
                    {
                        resultLabel.Text = $"Победил {(winner == 1 ? "крестик" : "нолик")}!";
                        PaintWinSet();
                        EndGame();
                        return;
                    }
                    else if (IsBoardFull())
                    {
                        resultLabel.Text = "Ничья!";
                        EndGame();

                        return;
                    }
                }
            }


            

        }

        // Проверка заполненности игрового поля (для ничьей)
        private bool IsBoardFull()
        {
            foreach (int cell in field)
            {
                if (cell == 0)
                    return false;
            }
            return true;
        }

        // Возвращает 1, если победили крестики, 2, если нолики, 0 - если победителя нет
        private int CheckWin()
        {
            // Проверка строк
            for (int i = 0; i < 3; i++)
            {
                if (field[i, 0] != 0 && field[i, 0] == field[i, 1] && field[i, 1] == field[i, 2])
                {
                    win_stack[i, 0] = 1;
                    win_stack[i, 1] = 1;
                    win_stack[i, 2] = 1;
                    return field[i, 0];
                }
                   
            }
            // Проверка столбцов
            for (int j = 0; j < 3; j++)
            {
                if (field[0, j] != 0 && field[0, j] == field[1, j] && field[1, j] == field[2, j])
                {
                   
                    return field[0, j];
                }
                    
            }
            // Проверка диагоналей
            if (field[0, 0] != 0 && field[0, 0] == field[1, 1] && field[1, 1] == field[2, 2])
                return field[0, 0];
            if (field[0, 2] != 0 && field[0, 2] == field[1, 1] && field[1, 1] == field[2, 0])
                return field[0, 2];

            

            return 0; // Победителя нет


        }


        private bool PreventWin()
        {
            bool res = false;
            // Проверка строк
            for (int i = 0; i < 3; i++)
            {
                count = 0;

                for (int j = 0; j < 3; j++)
                {
                    if (field[i, j] == 1)
                    {
                        count += 1;
                    }

                    else if (field[i, j] == 0)
                    {
                        position = i.ToString() + j.ToString();
                    }
                }

                if (count == 2)
                {
                  
                    res = true;
                    return res;
                }

            }
            return res;
        }

        private void PaintWinSet()
        {
            for (int i = 0; i <= win_stack.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= win_stack.GetUpperBound(0); j++)
                {
                    if (win_stack[i, j] == 1)
                    {
                        pictures[i, j].BackColor = Color.Green;
                    }
                }
            }
        }

        // Завершает игру, запрещая дальнейшие ходы и отображая кнопку рестарта
        private void EndGame()
        {
            gameOver = true;
            restartButton.Visible = true;
        }

        // Обработчик кнопки рестарта
        private void RestartButton_Click(object sender, EventArgs e)
        {
            ResetGame();
        }

        // Сброс игры до начального состояния
        private void ResetGame()
        {
            player = 1;
            loc_x = 120;
            loc_y = 50;
            gameOver = false;
            field = new int[3, 3];
            resultLabel.Text = "";
            restartButton.Visible = false;

            // Перебираем все контролы и сбрасываем PictureBox
            foreach (Control control in this.Controls)
            {
                if (control is PictureBox picture)
                {
                    picture.Image = Properties.Resources.template;
                    picture.Tag = "empty";
                }
            }
        }
    }
}