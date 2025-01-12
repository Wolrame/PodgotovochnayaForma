using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PodgotovochnayaForma
{
    public partial class Form1 : Form
    {
        public static int flag = 0;
        class Ball
        {
            public int x, y;
            public int xSpeed, ySpeed;
            public int w,h;
            public bool inCube;
            public delegate void DlTp();
            public DlTp dl;
            public Thread thr;
            void ThreadFunc()
            {
                while (true)
                {
                    if(inCube||(flag<4)){
                        if (x < 0 || x > 200) xSpeed = -xSpeed;
                        if (y < 0 || y > 200) ySpeed = -ySpeed;
                        //здесь пересчитываем координаты
                        x += xSpeed;
                        y += ySpeed;
                        Thread.Sleep(100);//спим
                        dl();
                    }
                }
            }
            public void DrawBall(Graphics dc)
            {
                dc.DrawEllipse(Pens.Red, x, y, w, h);
            }
            public Ball(int x, int y, int xSpeed, int ySpeed, int w, int h)
            {
                this.x = x;
                this.y = y;
                this.xSpeed = xSpeed;
                this.ySpeed = ySpeed;
                this.w = w;
                this.h = h;
                this.inCube = false;
                thr = new Thread(new ThreadStart(ThreadFunc));
                thr.Start();
            }
        }
        Ball[] bl = new Ball[5];
        public Form1()
        {
            InitializeComponent();
            for (int j = 0; j < bl.Length; j++)
            {
                //создаем потоковые объекты
                bl[j] = new Ball(j+1, j * 10, j + 1, j + 1, 10, 10);
                //подписываемся на событие
                bl[j].dl += new Ball.DlTp(Invalidate);
            }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.Aquamarine, 30, 30, 50, 50);

            for (int j = 0; j < bl.Length; j++)         
            {
                bl[j].DrawBall(e.Graphics);//рисуем
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int j = 0; j < bl.Length; j++) {
                if (new Rectangle(30,30,50,50).IntersectsWith(new Rectangle(bl[j].x, bl[j].y, bl[j].w, bl[j].h))) 
                {
                    flag++;
                    bl[j].inCube = true;
                }
                else 
                {
                    if (bl[j].inCube) flag--;
                    bl[j].inCube = false;
                }
            }
        }
    }
}
