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
        bool start_game = false;
        // Текущий игрок: 1 - крестики, 2 - нолики
        int player = 1;
        int computer = 2;
        Random random = new Random();
        int loc_x = 120;
        int loc_y = 50;
        Image image = null;
        string symbol = null;
        bool turn = true; // true очередь игрока / false очередь компьютера
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
            if (start_game)
            {
                // Если игра окончена, ходы не обрабатываем
                if (gameOver) return;

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

                if (turn)
                {
                    turn = false;
                    Move_PC();
                }
                else
                {
                    turn = true;
                }
            }
        }

        // Измененный метод для хода ИИ, чтобы ИИ ходил один раз
        private void Move_PC()
        {
            // Пока ходит ИИ, мы не хотим выполнять другие действия
            if (gameOver) return;

            int row_random = 0;
            int col_random = 0;
            bool key = true; // true — проверка осуществляется, false — проверка прекращается

            // Если компьютер может выиграть, он будет играть для победы
            while (key == true)
            {
                chance_win = PreventWin();
                if (chance_win == true)
                {
                    row_random = Convert.ToInt32(position[0].ToString());
                    col_random = Convert.ToInt32(position[1].ToString());
                    field[row_random, col_random] = computer;
                    pictures[row_random, col_random].Tag = symbol;
                    pictures[row_random, col_random].Image = image;
                    key = false;
                    break;
                }

                // Если клетка свободна, компьютер выбирает случайную клетку
                row_random = random.Next(0, 3);
                col_random = random.Next(0, 3);

                if (field[row_random, col_random] == 0)
                {
                    field[row_random, col_random] = computer;
                    pictures[row_random, col_random].Tag = symbol;
                    pictures[row_random, col_random].Image = image; // Change
                    key = false;
                }
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


            if (turn)
            {
                turn = false;
            }
            else
            {
                turn = true;
            }

        }


        private bool PreventWin()
        {
            bool res = false;

            // проверка главной диагонали
            count = 0;
            position = "";
            for (int i = 0; i < 3; i++)
            {
                if (field[i, i] == 1)
                {
                    count++;
                }
                else if (field[i, i] == 0) // Пустая клетка
                {
                    position = i.ToString() + i.ToString();
                }
            }
            // Если два крестика в строке и есть пустая клетка
            if (count == 2 && position != "")
            {
                res = true;
                return res; // Блокируем победу крестиков
            }


            // проверка побочной диагонали
            count = 0;
            position = "";
            int k = 2;
            for (int i = 0; i < 3; i++)
            {
                if (field[i, k] == 1)
                {
                    count++;
                }
                else if (field[i, k] == 0) // Пустая клетка
                {
                    position = i.ToString() + k.ToString();
                }
                k--;
            }
            // Если два крестика в строке и есть пустая клетка
            if (count == 2 && position != "")
            {
                res = true;
                return res; // Блокируем победу крестиков
            }

            // Проверка строк на блокирование
            for (int i = 0; i < 3; i++)
            {
                count = 0;
                position = "";
                for (int j = 0; j < 3; j++)
                {
                    if (field[i, j] == 1) // Крестик
                    {
                        count++;
                    }
                    else if (field[i, j] == 0) // Пустая клетка
                    {
                        position = i.ToString() + j.ToString();
                    }
                }

                // Если два крестика в строке и есть пустая клетка
                if (count == 2 && position != "")
                {
                    res = true;
                    return res; // Блокируем победу крестиков
                }
            }

            // Проверка столбцов на блокирование
            for (int j = 0; j < 3; j++)
            {
                count = 0;
                position = "";
                for (int i = 0; i < 3; i++)
                {
                    if (field[i, j] == 1) // Крестик
                    {
                        count++;
                    }
                    else if (field[i, j] == 0) // Пустая клетка
                    {
                        position = i.ToString() + j.ToString();
                    }
                }

                // Если два крестика в столбце и есть пустая клетка
                if (count == 2 && position != "")
                {
                    res = true;
                    return res; // Блокируем победу крестиков
                }
            }

           

            return res;
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
                    win_stack[i, 0] = player;
                    win_stack[i, 1] = player;
                    win_stack[i, 2] = player;
                    return field[i, 0];
                }
            }

            // Проверка столбцов
            for (int j = 0; j < 3; j++)
            {
                if (field[0, j] != 0 && field[0, j] == field[1, j] && field[1, j] == field[2, j])
                {
                    win_stack[0, j] = 1;
                    win_stack[1, j] = 1;
                    win_stack[2, j] = 1;
                    return field[0, j];
                }
            }

            // Проверка диагоналей
            if (field[0, 0] != 0 && field[0, 0] == field[1, 1] && field[1, 1] == field[2, 2])
            {
                win_stack[0, 0] = 1;
                win_stack[1, 1] = 1;
                win_stack[2, 2] = 1;
                return field[0, 0];
            }

            if (field[0, 2] != 0 && field[0, 2] == field[1, 1] && field[1, 1] == field[2, 0])
            {
                win_stack[0, 2] = 1;
                win_stack[1, 1] = 1;
                win_stack[2, 0] = 1;
                return field[0, 2];
            }

            return 0; // Победителя нет
        }

        private void PaintWinSet()
        {
            // Сброс всех подсветок перед новой игрой
            foreach (Control control in this.Controls)
            {
                if (control is PictureBox picture)
                {
                    picture.BackColor = Color.Transparent;
                }
            }

            // Подсветка победивших клеток
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (win_stack[i, j] == 1)
                    {
                        pictures[i, j].BackColor = Color.Green;
                    }
                }
            }
        }

        private void EndGame()
        {
            // Включаем кнопку рестарта после окончания игры
            restartButton.Visible = true;
            gameOver = true;
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            // Сбрасываем поле и все настройки для новой игры
            Array.Clear(field, 0, field.Length);
            Array.Clear(win_stack, 0, win_stack.Length);
            foreach (Control control in this.Controls)
            {
                if (control is PictureBox picture)
                {
                    picture.Image = Properties.Resources.template;
                    picture.Tag = "empty";
                    picture.BackColor = Color.Transparent;
                }
            }
            resultLabel.Text = "";
            restartButton.Visible = false;
            player = 1;
            gameOver = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            start_game = true;
            if (comboBox1.Text == "Нолик")
            {
                player = 2;
                turn = false;
                computer = 1;
                image = Properties.Resources.Крестик;
                symbol = "X";
                Move_PC();
            }
            else
            {
                player = 1;
                turn = true;
                computer = 2;
                image = Properties.Resources.Нолик;
                symbol = "O";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Вы выбрали - " + comboBox1.Text);
        }
    }
}

