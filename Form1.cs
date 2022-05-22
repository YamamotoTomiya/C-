using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Dynamic;
using System.IO;
using System.Windows;

namespace kabetosi
{
    public partial class Form1 : Form
    {
        Vector[] blockSpeed1 = new Vector[10];
        Vector[] blockPos1 = new Vector[10];
        Vector[] blockSpeed2 = new Vector[10];
        Vector[] blockPos2 = new Vector[10];
        Vector[] ballPos = new Vector[2];

        Random r = new Random();

        int r1 = 0;
        int r2 = 0;
        int r3 = 0;

        int a = 0;
        int c = 0;
        int e = 0;

        Timer timer1 = new Timer();
        public Form1()
        {
            InitializeComponent();

            r1 = r.Next(250);
            r2 = r.Next(250);
            r3 = r.Next(250);

            a = r1 + 60;
            c = r2 + 60;
            e = r3 + 60;

            this.ballPos[1] = new Vector(20, 100);

            this.blockPos1[1] = new Vector(Width, 0);//壁の位置
            this.blockSpeed1[1] = new Vector(-2, 0);
            this.blockPos2[1] = new Vector(Width, a);//壁の位置
            this.blockSpeed2[1] = new Vector(-2, 0);

            this.blockPos1[2] = new Vector(Width - 170, 0);//壁の位置
            this.blockSpeed1[2] = new Vector(-2, 0);
            this.blockPos2[2] = new Vector(Width - 170, c);//壁の位置
            this.blockSpeed2[2] = new Vector(-2, 0);

            this.blockPos1[3] = new Vector(Width - 340, 0);//壁の位置
            this.blockSpeed1[3] = new Vector(-2, 0);
            this.blockPos2[3] = new Vector(Width - 340, e);//壁の位置
            this.blockSpeed2[3] = new Vector(-2, 0);

            timer1.Interval = 20;
            timer1.Tick += new EventHandler(Update1);
            timer1.Start();

        }
        private void Update1(object sender, EventArgs e)
        {

            // ボールの移動
            blockPos1[1] += blockSpeed1[1];
            blockPos2[1] += blockSpeed2[1];
            blockPos1[2] += blockSpeed1[2];
            blockPos2[2] += blockSpeed2[2];
            blockPos1[3] += blockSpeed1[3];
            blockPos2[3] += blockSpeed2[3];

            if (blockPos2[1].Y - 40 > ballPos[1].Y && blockPos1[1].X == ballPos[1].X + 20)
            {
                timer1.Stop();
            }
            if (blockPos2[1].Y < ballPos[1].Y && blockPos1[1].X == ballPos[1].X + 20)
            {
                timer1.Stop();
            }
            if (blockPos2[2].Y - 40 > ballPos[1].Y && blockPos1[2].X == ballPos[1].X + 20)
            {
                timer1.Stop();
            }
            if (blockPos2[2].Y < ballPos[1].Y && blockPos1[2].X == ballPos[1].X + 20)
            {
                timer1.Stop();
            }
            if (blockPos2[3].Y - 40 > ballPos[1].Y && blockPos1[3].X == ballPos[1].X + 20)
            {
                timer1.Stop();
            }
            if (blockPos2[3].Y < ballPos[1].Y && blockPos1[3].X == ballPos[1].X + 20)
            {
                timer1.Stop();
            }
            if (blockPos1[1].X < 0)
            {
                blockPos1[1].X = Width;
                blockPos2[1].X = Width;
                int r4 = r.Next(250);
                int a2 = 0;
                a2 = r4 + 60;
                blockPos2[1].Y = a2;
            }
            if (blockPos1[2].X < 0)
            {
                blockPos1[2].X = Width;
                blockPos2[2].X = Width;
                int r5 = r.Next(250);
                int c2 = 0;
                c2 = r5 + 60;
                blockPos2[2].Y = c2;
            }
            if (blockPos1[3].X < 0)
            {
                blockPos1[3].X = Width;
                blockPos2[3].X = Width;
                int r6 = r.Next(250);
                int e2 = 0;
                e2 = r6 + 60;
                blockPos2[3].Y = e2;
            }
            // 再描画
            Invalidate();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Draw(object sender, PaintEventArgs e)
        {

            SolidBrush pinkBrush = new SolidBrush(Color.LightPink);
            SolidBrush blueBrush = new SolidBrush(Color.OrangeRed);

            float bx1 = (float)this.ballPos[1].X;
            float by1 = (float)this.ballPos[1].Y;

            float px1 = (float)this.blockPos1[1].X;
            float py1 = (float)this.blockPos1[1].Y;
            float px2 = (float)this.blockPos2[1].X;
            float py2 = (float)this.blockPos2[1].Y;
            float px3 = (float)this.blockPos1[2].X;
            float py3 = (float)this.blockPos1[2].Y;
            float px4 = (float)this.blockPos2[2].X;
            float py4 = (float)this.blockPos2[2].Y;
            float px5 = (float)this.blockPos1[3].X;
            float py5 = (float)this.blockPos1[3].Y;
            float px6 = (float)this.blockPos2[3].X;
            float py6 = (float)this.blockPos2[3].Y;

            e.Graphics.FillRectangle(pinkBrush, bx1, by1, 20, 20);

            e.Graphics.FillRectangle(blueBrush, px1, py1, 5, py2 - 40);
            e.Graphics.FillRectangle(blueBrush, px2, py2, 5, 1000);
            e.Graphics.FillRectangle(blueBrush, px3, py3, 5, py4 - 40);
            e.Graphics.FillRectangle(blueBrush, px4, py4, 5, 1000);
            e.Graphics.FillRectangle(blueBrush, px5, py5, 5, py6 - 40);
            e.Graphics.FillRectangle(blueBrush, px6, py6, 5, 1000);
        }

        private void KeyPressed(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == 'a')
            {
                this.ballPos[1].Y += 10;
            }
            else if (e.KeyChar == 'd')
            {
                this.ballPos[1].Y -= 10;
            }
        }
    }
}
