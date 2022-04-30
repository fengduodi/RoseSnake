using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace 玫瑰蛇.Class
{
    class Food
    {
        int x, y, size;

        Image image;

        public List<SnackBody> snack;

        public Food()
        {
            size = 40;

            image = Image.FromFile(".\\image\\food.png");
            image = new Bitmap(image, new Size(size, size));
        }

        public void Check()
        {
            if (Math.Sqrt(Math.Pow(x - snack[0].x, 2) + Math.Pow(y - snack[0].y, 2)) < 25)
            {
                snack.Add(new Body(snack[4].x, snack[4].y, snack[4].forward_x, snack[4].forward_y));
                snack[snack.Count - 1].signs = snack[4].signs;

                snack[4].x -= snack[4].forward_x * 5;
                snack[4].y -= snack[4].forward_y * 5;

                Reset();
            }
        }

        public void Reset()
        {
            Random random = new Random();

            int range = Form1.backgroundSize / 2 - size / 2 - 10;

            x = random.Next(-range, range);
            y = random.Next(-range, range);
        }

        public void Show(PaintEventArgs e)
        {
            Graphics G = e.Graphics;

            int x2 = Form1.windowsWidth / 2 - size / 2 + x;
            int y2 = Form1.windowsHeight / 2 - size / 2 + y;

            G.DrawImage(image, x2, y2);
        }
    }
}