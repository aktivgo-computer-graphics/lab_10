using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net;
using System.Windows.Forms;

namespace task
{
    public partial class MainForm : Form
    {
        private Pen MyPen;
        private Bitmap bmp;
        private Graphics graph;

        public MainForm()
        {
            InitializeComponent();
            MyPen = new Pen(Color.Black);
            bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
            graph = Graphics.FromImage(bmp);
            graph.SmoothingMode = SmoothingMode.HighQuality;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            graph = Graphics.FromImage(bmp);
            graph.Clear(Color.White);
            Waves(1000);
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            graph = Graphics.FromImage(bmp);
            graph.Clear(Color.White);
            Roys(500);
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            graph = Graphics.FromImage(bmp);
            graph.Clear(Color.White);
            Roys(500);
        }

        private void Waves(int n)
        {
            Random rand = new Random();
            List<int> X = new List<int>();
            List<int> Y = new List<int>();
            List<int> P = new List<int>();
            List<int> vX = new List<int>();
            List<int> vY = new List<int>();
            
            List<int> X2 = new List<int>();
            List<int> Y2 = new List<int>();
            List<int> P2 = new List<int>();
            List<int> vX2 = new List<int>();
            List<int> vY2 = new List<int>();
            
            for (int i = 0; i < n; i++)
            {
                X.Add(rand.Next(ClientSize.Width));
                Y.Add(rand.Next(40));
                P.Add(rand.Next(255));
                vX.Add(-2 + rand.Next(5));
                vY.Add(rand.Next(7) + 9 + 10);
                
                X2.Add(rand.Next(40));
                Y2.Add(rand.Next(ClientSize.Height));
                P2.Add(rand.Next(255));
                vX2.Add(rand.Next(7) + 9 + 10);
                vY2.Add(-2 + rand.Next(5));
            }

            for (int iter = 0; iter < 500; iter++)
            {
                graph.DrawImage(bmp, 0, 0);

                bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
                graph.Clear(Color.White);
                for (int i = 0; i < n; i++)
                {
                    if (X[i] + vX[i] <= 0 || X[i] + vX[i] >= ClientSize.Width)
                    {
                        vX[i] = -vX[i];
                    }
                    if (Y[i] + vY[i] <= 0 || Y[i] + vY[i] >= ClientSize.Height)
                    {
                        vY[i] = -vY[i];
                    }
                    
                    bmp.SetPixel(Math.Abs(X[i] % ClientSize.Width), Math.Abs(Y[i] % ClientSize.Height), Color.White);
                    X[i] += vX[i];
                    Y[i] += vY[i];
                    bmp.SetPixel(Math.Abs(X[i] % ClientSize.Width), Math.Abs(Y[i] % ClientSize.Height), Color.FromArgb(0, 0, P[i]));
                    
                    if (X2[i] + vX2[i] <= 0 || X2[i] + vX2[i] >= ClientSize.Width)
                    {
                        vX2[i] = -vX2[i];
                    }
                    if (Y2[i] + vY2[i] <= 0 || Y2[i] + vY2[i] >= ClientSize.Height)
                    {
                        vY2[i] = -vY2[i];
                    }
                    
                    bmp.SetPixel(Math.Abs(X2[i] % ClientSize.Width), Math.Abs(Y2[i] % ClientSize.Height), Color.White);
                    X2[i] += vX2[i];
                    Y2[i] += vY2[i];
                    bmp.SetPixel(Math.Abs(X2[i] % ClientSize.Width), Math.Abs(Y2[i] % ClientSize.Height), Color.FromArgb(0, P2[i], 0));
                }
            }
        }

        private void Roys(int n)
        {
            
            Random rand = new Random();
            List<int> X = new List<int>();
            List<int> Y = new List<int>();
            int xc = ClientSize.Width / 2;
            int yc = ClientSize.Height / 2;
            int vxc = 10;
            
            for (int i = 0; i < n; i++)
            {
                X.Add(rand.Next(ClientSize.Width));
                Y.Add(rand.Next(40));
            }

            for (int iter = 0; iter < 1000; iter++)
            {
                bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
                graph.Clear(Color.White);
                
                if (xc > ClientSize.Width || xc < 0)
                {
                    vxc = -vxc;
                    xc += vxc;
                }

                for (int i = 0; i < n; i++)
                {
                    int dx = -25 + rand.Next(51);
                    int dy = -25 + rand.Next(51);
                    
                    bmp.SetPixel(Math.Abs(X[i] % ClientSize.Width), Math.Abs(Y[i] % ClientSize.Height), Color.White);

                    if (X[i] + dx > 0 && X[i] + dx < ClientSize.Width)
                    {
                        if (X[i] + dx < xc)
                        {
                            dx += 5;
                        }
                        else
                        {
                            dx -= 5;
                        }

                        X[i] += dx;
                    }
                    
                    if (Y[i] + dy > 0 && Y[i] + dy < ClientSize.Height)
                    {
                        if (Y[i] + dy < yc)
                        {
                            dy += 5;
                        }
                        else
                        {
                            dy -= 5;
                        }

                        Y[i] += dy;
                    }

                    bmp.SetPixel(Math.Abs(X[i] % ClientSize.Width), Math.Abs(Y[i] % ClientSize.Height), Color.Black);
                }
                
                graph.DrawImage(bmp, 0, 0);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            MyPen.Dispose();
            bmp.Dispose();
            graph.Dispose();
        }
    }
}