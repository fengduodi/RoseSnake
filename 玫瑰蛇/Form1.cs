using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using 玫瑰蛇.Class;

namespace 玫瑰蛇
{
    public partial class Form1 : Form
    {
        int bias;
        public static int windowsWidth, windowsHeight;

        public static int backgroundSize;

        Image backgroundImage;

        List<SnackBody> snack;

        List<Sign> signs;

        Food food;

        Timer updateTimer;

        bool keyup;

        public Form1()
        {
            InitializeComponent();

            Size = new Size(800 , 800);

            bias = 15;
            windowsWidth = Width - bias;
            windowsHeight = Height - SystemInformation.ToolWindowCaptionHeight - bias;

            backgroundSize = 700;

            backgroundImage = Image.FromFile(".\\image\\background.jpg");
            backgroundImage = new Bitmap(backgroundImage, new Size(backgroundSize, backgroundSize));

            snack = new List<SnackBody>();
            signs = new List<Sign>();

            food = new Food();
            food.snack = snack;

            Setting();

            updateTimer = new Timer();
            updateTimer.Interval = 10;
            updateTimer.Tick += new EventHandler(Run);
            updateTimer.Enabled = true;

            keyup = false;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics G = e.Graphics;

            G.DrawImage(backgroundImage, windowsWidth / 2 - backgroundSize / 2, windowsHeight / 2 - backgroundSize / 2);

            for (int i = snack.Count - 1; i >= 0; i--)
                snack[i].Show(e);

            food.Show(e);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up && snack[0].forward_y == 0)
                signs.Add(new Sign(snack[0].x, snack[0].y, 0, -5));

            if (e.KeyCode == Keys.Down && snack[0].forward_y == 0)
                signs.Add(new Sign(snack[0].x, snack[0].y, 0, 5));

            if (e.KeyCode == Keys.Left && snack[0].forward_x == 0)
                signs.Add(new Sign(snack[0].x, snack[0].y, -5, 0));

            if (e.KeyCode == Keys.Right && snack[0].forward_x == 0)
                signs.Add(new Sign(snack[0].x, snack[0].y, 5, 0));

            if (keyup)
                keyup = false;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            keyup = true;
        }

        public void Run(object sender, EventArgs e)
        {
            foreach (var body in snack)
                body.Move();

            food.Check();

            if (GameOver())
            {
                updateTimer.Stop();

                DialogResult dialogResult = MessageBox.Show("玫瑰蛇的長度：" + snack.Count.ToString() + "\n\n再玩一次？",
                                                            "遊戲結束",
                                                            MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    Setting();

                    updateTimer.Start();
                }
                else if (dialogResult == DialogResult.No)
                {
                    Environment.Exit(0);
                }
            }

            Invalidate();
        }

        bool GameOver()
        {
            int border = backgroundSize / 2 - snack[0].size / 2;

            if (snack[0].x < -border || snack[0].x > border ||
                snack[0].y < -border || snack[0].y > border)
                return true;

            for (int i = 1; i < snack.Count; i++)
                if (Math.Sqrt(Math.Pow(snack[0].x - snack[i].x, 2) + Math.Pow(snack[0].y - snack[i].y, 2)) < 10)
                    return true;

            return false;
        }

        void Setting()
        {
            snack.Clear();
            signs.Clear();

            snack.Add(new Head(0, 0, 5, 0));
            snack.Add(new Body(-30, 0, 5, 0));
            snack.Add(new Body(-55, 0, 5, 0));
            snack.Add(new Body(-80, 0, 5, 0));
            snack.Add(new Tail(-105, 0, 5, 0));

            foreach (var body in snack)
                body.signs = signs;

            food.Reset();
        }
    }
}