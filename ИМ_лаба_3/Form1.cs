using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ИМ_лаба_3
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        int resolution = 5;
        private bool[,] field;
        private int rows;
        private int cols;
        int iRows;
        int rule;
        bool[] rules = new bool[8];
        bool left, center, right;
        int index;
        public Form1()
        {

            InitializeComponent();
        }
        private void DrawField()
        {
            Pen borderPen = new Pen(Color.Black, 2);
            graphics.DrawRectangle(borderPen, 0, 0, cols * resolution, rows * resolution);

            // Рисуем сетку игрового поля
            for (int i = 1; i < rows; i++)
            {
                graphics.DrawLine(Pens.Black, 0, i * resolution, cols * resolution, i * resolution);
            }

            for (int j = 1; j < cols; j++)
            {
                graphics.DrawLine(Pens.Black, j * resolution, 0, j * resolution, rows * resolution);
            }

            
        }

        private void FirstGeneration() {
            for (int i = 0; i < cols; ++i)
            {
                if (field[i, 0] == true)
                {
                    graphics.FillRectangle(Brushes.Red, i * resolution + 1, 0 * resolution + 1, resolution - 1, resolution-1);

                }
            }

        }
        private void NextGeneration(int j, int iRows)
        {
            for (int i = 0; i < cols; ++i)
            {
                if ((field[i, iRows] == true) && (iRows < rows))
                {
                    graphics.FillRectangle(Brushes.Red, i * resolution + 1, iRows * resolution + 1, resolution - 1, resolution - 1);

                }
            }

        }
        private void StartGame()
        {
        if (timer1.Enabled == true) return;

            rule = (int)edRule.Value; 
            cols = pictureBox1.Width / resolution; 
            rows = pictureBox1.Height / resolution;
            
            iRows = 1;

            field = new bool[cols, rows];
            Random random = new Random();
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    field[i, j] = false;
                }
            }

            for (int i = 1; i < cols; i++) 
            {
                 field[i, 0] = random.Next(2) == 0;
            }

            for (int i = 0; i < 8; i++)
            {
                rules[i] = (rule & (1 << i)) != 0;
            }
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);


            // Рисуем сетку игрового поля
            DrawField();
            FirstGeneration();


            timer1.Start();

        }
   
        private void Myrules()
        {
            if(left && center && right) { index  = 0; }
            else if (left && center && !right) { index = 1; }
            else if (left && !center && right) { index = 2; }
            else if (left && !center && !right) { index = 3; }
            else if (!left && center && right) { index = 4; }
            else if (!left && center && !right) { index = 5; }
            else if (!left && !center && right) { index = 6; }
            else if (!left && !center && !right) { index = 7; }
        }
        


        private void timer1_Tick(object sender, EventArgs e)
        {

            for (int j = 0; j < cols; j++)
            {
                if (j == 0 && iRows < rows)
                {
                    left = field[cols - 1, iRows-1];
                    center = field[j, iRows-1];
                    right = field[j + 1, iRows - 1];
                }
                else if (j == cols - 1)
                {
                    left = field[j - 1, iRows - 1];
                    center = field[j, iRows - 1];
                    right = field[0, iRows - 1];
                }
                else if (iRows < rows)
                {
                    left = field[j - 1, iRows - 1];
                    center = field[j, iRows - 1];
                    right = field[j + 1, iRows - 1];
                }

                Myrules();
                field[j, iRows] = rules[index];

                NextGeneration(j, iRows);
                pictureBox1.Invalidate();
                

            }
            if (iRows < rows - 1) iRows += 1;
            else timer1.Stop();


            
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            
            
        }
    }
}
