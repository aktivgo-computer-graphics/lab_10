using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace task
{
    public partial class MainForm : Form
    {
        private Pen MyPen;

        private int task = -1;
        
        public MainForm()
        {
            InitializeComponent();
            MyPen = new Pen(Color.Black);
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            task = 1;
            Invalidate();
        }
        
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            switch (task)
            {
                case 1:
                    Waves(e.Graphics, 500);
                    break;
            }
        }

        private void Waves(Graphics graph, int n)
        {
            Random rand = new Random();
            List<int> X = new List<int>();
            List<int> Y = new List<int>();
            List<int> P = new List<int>();
            List<int> vX = new List<int>();
            List<int> vY = new List<int>();
            
            for (int i = 0; i < n; i++)
            {
                X.Add(rand.Next(ClientSize.Width));
                Y.Add(rand.Next(40));
                P.Add(rand.Next(7) + 9);
                vX.Add(-2 + rand.Next(5));
                vY.Add(P[i] - 6);
            }

            for (int iter = 0; iter < 1000; iter++)
            {
                for (int i = 0; i < n; i++)
                {
                    if (X[i] + vX[i] < 0 || X[i] + vX[i] > ClientSize.Width)
                    {
                        vX[i] = -vX[i];
                    }
                    if (Y[i] + vY[i] < 0 || Y[i] + vY[i] > ClientSize.Height)
                    {
                        vY[i] = -vY[i];
                    }
                    
                    /*MyPen.Color = Color.White;
                    graph.DrawRectangle(MyPen, X[i], Y[i], 1, 1);*/
                    graph.Clear(Color.White);
                    X[i] += 1;
                    vX[i] += 1;
                    Y[i] += 1;
                    vY[i] += 1;
                    MyPen.Color = Color.FromArgb(0, 0, P[i]);
                    graph.DrawRectangle(MyPen, X[i], Y[i], 1, 1);
                    MyPen.Color = Color.White;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            MyPen.Dispose();
        }
    }
}