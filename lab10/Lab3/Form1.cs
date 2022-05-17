using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3
{
    public partial class MainForm : Form
    {
        Graphics g;
        Pen pen;

        private static int task;

        //task 1
        private static int pointCount, groupCount;
        private static List<int> speedY;
        private static List<List<Point>> pointsY;
        private static List<int> speedX;
        private static List<List<Point>> pointsX;
        private static Random random;
        private static Bitmap bitmap;
        
        //task 2
        private static List<PointOfRoy> points;
        private static List<Point> target;
        
        //task3
        private static List<PointF> points3;
        private static float speedX3, speedY3;
        private static bool forX;
        Graphics g3;
        
        //task4
        int Gx, Gy, N;
        float L;
        bool stop;

        List<bool> good = new List<bool>();
        List<float> R = new List<float>();
        List<float> dR = new List<float>();
        List<float> xc = new List<float>();
        List<float> yc = new List<float>();
        Random rand = new Random();

        public MainForm()
        {
            InitializeComponent();
            g = CreateGraphics();
            pen = new Pen(Color.Black);
            pen.Width = 3;

            var timer = new Timer();
            timer.Interval = 1;
            timer.Tick += timer_tick;
            timer.Enabled = true;

            g.SmoothingMode = SmoothingMode.HighQuality;

            bitmap = new Bitmap(ClientSize.Width, ClientSize.Height);
        }

        private static void ClearImage()
        {
            var g = Graphics.FromImage(bitmap);
            g.Clear(Color.White);
        }

        private void timer_tick(object sender, EventArgs e)
        {
            ClearImage();

            switch (task)
            {
                case 1:
                {
                    for (var i = 0; i < groupCount; i++)
                    {
                        for (var j = 0; j < pointCount; j++)
                        {
                            pointsY[i][j] = new Point(pointsY[i][j].X + random.Next(-5, 5), pointsY[i][j].Y + speedY[i]);
                            pointsX[i][j] = new Point(pointsX[i][j].X + speedX[i], pointsX[i][j].Y + random.Next(-5, 5));

                            bitmap.SetPixel(Math.Abs(pointsY[i][j].X) % ClientSize.Width, Math.Abs(pointsY[i][j].Y) % ClientSize.Height, Color.Red);
                            bitmap.SetPixel(Math.Abs(pointsX[i][j].X) % ClientSize.Width, Math.Abs(pointsX[i][j].Y) % ClientSize.Height, Color.Blue);
                        }
                    }

                    g.DrawImage(bitmap, 0, 0);

                    for (var i = 0; i < groupCount; i++)
                    {
                        if (pointsY[i][0].Y + speedY[i] > ClientSize.Height) speedY[i] = speedY[i] > 0 ? speedY[i] = -speedY[i] : speedY[i];
                        else if (pointsY[i][0].Y - speedY[i] < 0) speedY[i] = speedY[i] <= 0 ? speedY[i] = -speedY[i] : speedY[i];

                        if (pointsX[i][0].X + speedX[i] > ClientSize.Width) speedX[i] = speedX[i] > 0 ? speedX[i] = -speedX[i] : speedX[i];
                        else if (pointsY[i][0].X - speedX[i] < 0) speedX[i] = speedX[i] <= 0 ? speedX[i] = -speedX[i] : speedX[i];
                    }

                    return;
                }
                case 2:
                {
                    float speed = 0.3f, speedPoint = 1f;
                    for (var i = 0; i < pointCount; i++)
                    {
                        if (points[i].X < target[i < pointCount / 2 ? 0 : 1].X)
                        {
                            if (points[i].SpeedX < 5f) points[i].SpeedX += speed + (float)(random.Next(-1, 1) < 0 ? -random.NextDouble() : random.NextDouble());
                        }
                        else if (points[i].SpeedX > -5f)
                        {
                            points[i].SpeedX -= speed + (float)(random.Next(-1, 1) < 0 ? -random.NextDouble() : random.NextDouble());
                        }

                        if (points[i].Y < target[i < pointCount / 2 ? 0 : 1].Y)
                        {
                            if (points[i].SpeedY < 5f) points[i].SpeedY += speed + (float)(random.Next(-1, 1) < 0 ? -random.NextDouble() : random.NextDouble());
                        }
                        else if (points[i].SpeedY > -5f) points[i].SpeedY -= speed + (float)(random.Next(-1, 1) < 0 ? -random.NextDouble() : random.NextDouble());

                        points[i].X += points[i].SpeedX;
                        points[i].Y += points[i].SpeedY;

                        bitmap.SetPixel((int)Math.Abs(points[i].X) % ClientSize.Width, (int)Math.Abs(points[i].Y) % ClientSize.Height, i < pointCount / 2 ? Color.Green : Color.Chocolate);
                    }

                    if (target[1].X >= 1200) forX = false;
                    else if (target[1].X <= 100) forX = true;

                    target[1] = new Point((int)(forX ? target[1].X + speedPoint : target[1].X - speedPoint), ClientSize.Height / 2);

                    g.DrawImage(bitmap, 0, 0);

                    return;
                }
                case 3:
                {
                    for (var i = 0; i < pointCount; i++)
                    {
                        points3[i] = new PointF(
                            points3[i].X < ClientSize.Width / 2 ? 
                                points3[i].X - speedX3 * Math.Abs(ClientSize.Width / 2 - points3[i].X) / 200f : 
                                points3[i].X + speedX3 * Math.Abs(ClientSize.Width / 2 - points3[i].X) / 200f,
                            points3[i].Y < ClientSize.Height / 2 ?
                                points3[i].Y - Math.Abs(ClientSize.Height / 2 - points3[i].Y) / Math.Abs(ClientSize.Width / 2 - points3[i].X) * speedX3 * Math.Abs(ClientSize.Width / 2 - points3[i].X) / 200f :
                                points3[i].Y + Math.Abs(ClientSize.Height / 2 - points3[i].Y) / Math.Abs(ClientSize.Width / 2 - points3[i].X) * speedX3 * Math.Abs(ClientSize.Width / 2 - points3[i].X) / 200f
                        );

                        if (points3[i].X + speedX3 >= ClientSize.Width ||
                            points3[i].X - speedX3 <= 0 ||
                            points3[i].Y + speedY3 >= ClientSize.Height ||
                            points3[i].Y - speedY3 <= 0)
                        {
                            points3[i] = new PointF(random.Next(10, ClientSize.Width), random.Next(10, ClientSize.Height));
                        }

                        g3.FillEllipse(new SolidBrush(Color.OrangeRed), new RectangleF(points3[i], new SizeF(Math.Abs(ClientSize.Width / 2 - points3[i].X) / 50f, Math.Abs(ClientSize.Width / 2 - points3[i].X) / 50f)));
                        g3.FillEllipse(new SolidBrush(Color.Orange), new RectangleF(points3[i], new SizeF(Math.Abs(ClientSize.Height / 2 - points3[i].Y) / 50f, Math.Abs(ClientSize.Height / 2 - points3[i].Y) / 50f)));
                    }

                    g.DrawImage(bitmap, 0, 0);
                    
                    return;
                }
                case 4 when !stop:
                {
                    for (var i = 0; i < N; i++)
                    {
                        if (good[i])
                        {
                            g3.DrawEllipse(pen, new RectangleF(xc[i] - R[i], yc[i] - R[i], 2 * R[i], 2 * R[i]));
                            g3.FillEllipse(new SolidBrush(Color.LightCyan), new RectangleF(xc[i] - R[i], yc[i] - R[i], 2 * R[i], 2 * R[i]));
                            g3.FillEllipse(new SolidBrush(Color.White), new RectangleF((int)(xc[i] - R[i] + 0.1 * R[i]), (int)(yc[i] - R[i] + 0.5 * R[i]), R[i], R[i]));
                            for (var j = 0; j < N; j++)
                            {
                                if (j == i || !good[j]) continue;
                                L = (float)Math.Sqrt(Math.Pow(xc[i] - xc[j], 2) + Math.Pow(yc[i] - yc[j], 2));
                                if (!(R[i] + R[j] + dR[i] + dR[j] >= L) ||!(R[j] >= R[i])) continue;
                                System.Media.SystemSounds.Asterisk.Play();
                                good[i] = false;
                                break;
                            }
                        }

                        if (xc[i] - R[i] - dR[i] <= 0)
                        {
                            xc[i] += dR[i];
                        }
                    
                        if (yc[i] - R[i] - dR[i] <= 0)
                        {
                            yc[i] += dR[i];
                        }
                    
                        if (xc[i] + R[i] + dR[i] >= Gx)
                        {
                            xc[i] -= dR[i];
                        }
                    
                        if (yc[i] + R[i] + dR[i] >= Gy)
                        {
                            yc[i] -= dR[i];
                        }
                    }
                
                    for (var i = 0; i < N; i++)
                    {
                        if (good[i])
                        {
                            R[i] += dR[i];
                        }
                    
                        if (2 * R[i] > Gy || 2 * R[i] > Gx)
                        {
                            task = 0;
                            stop = true;
                        }
                    }

                    g.DrawImage(bitmap, 0, 0);
                    break;
                }
            }
        }
        
        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            target[0] = new Point(e.X, e.Y);
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            pointCount = 1000;
            groupCount = 5;

            speedY = new List<int>();
            pointsY = new List<List<Point>>();
            speedX = new List<int>();
            pointsX = new List<List<Point>>();
            random = new Random();

            for (var i = 0; i < groupCount; i++)
            {
                speedY.Add(random.Next(10));
                pointsY.Add(new List<Point>());
                speedX.Add(random.Next(10));
                pointsX.Add(new List<Point>());
                for (var j = 0; j < pointCount; j++)
                {
                    pointsY[i].Add(new Point(random.Next(10, ClientSize.Width), random.Next(100)));
                    pointsX[i].Add(new Point(random.Next(100), random.Next(10, ClientSize.Height)));
                }
            }

            task = 1;
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            pointCount = 10000;

            BackColor = Color.White;
            target = new List<Point>
            {
                new Point(ClientSize.Width / 2 - 100, ClientSize.Height / 2),
                new Point(ClientSize.Width / 2 + 100, ClientSize.Height / 2)
            };

            forX = true;

            points = new List<PointOfRoy>();
            random = new Random();

            for (var j = 0; j < pointCount; j++)
            {
                points.Add(new PointOfRoy(random.Next(10, ClientSize.Width), random.Next(10, ClientSize.Height)));
            }

            task = 2;
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            pointCount = 1000;
            speedX3 = 5;
            speedY3 = speedX3 / 3;
            
            g3 = Graphics.FromImage(bitmap);
            points3 = new List<PointF>();
            random = new Random();

            for (var j = 0; j < pointCount; j++)
            {
                points3.Add(new PointF(random.Next(10, ClientSize.Width), random.Next(10, ClientSize.Height)));
            }

            task = 3;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            N = 10;
            
            g3 = Graphics.FromImage(bitmap);
            Gx = ClientSize.Width;
            Gy = ClientSize.Height;

            for (var i = 0; i < N; i++)
            {
                R.Add(rand.Next(5, 15));
                dR.Add(rand.Next(1, 3));

                xc.Add(2 * R[i] + rand.Next(0, (int)(Gx - 4 * R[i])));
                yc.Add(2 * R[i] + rand.Next(0, (int)(Gy - 4 * R[i])));
                good.Add(true);
            }
            stop = false;

            task = 4;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            pen.Dispose();
            bitmap.Dispose();
            g.Dispose();
        }
    }
}