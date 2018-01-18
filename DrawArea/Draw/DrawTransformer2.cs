using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace DrawArea
{
    [Serializable]
    public class DrawTransformer2 : DrawRectanlge
    {
        public DrawTransformer2(int x, int y, int width, int height)
            : base(x, y, width, height)
        {
            base.Name = "Ë«ÈÆ±äÑ¹Æ÷";
        }
        public DrawTransformer2()
            : this(0, 0, 80, 40)
        {
            base.Name = "Ë«ÈÆ±äÑ¹Æ÷";
        }
        public override int NodeCount
        {
            get
            {
                return 2;
            }
        }
        public override Point GetNode(int NodeNumber)
        {
            int x = rectangle.Left;
            int y = rectangle.Top;
            switch (NodeNumber)
            {
                case 1:
                    x = rectangle.Left;
                    y = rectangle.Top + (int)(rectangle.Height / 2);
                    break;
                case 2:
                    x = rectangle.Right;
                    y = rectangle.Top + (int)(rectangle.Height / 2);
                    break;
            }

            return new Point(x, y);
        }
        public override void Draw(Graphics g)
        {
            base.Draw(g);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Pen pen = new Pen(Color, PenWidth);

            Rectangle rect = GetNormalizedRectangle(base.rectangle);
            int x = rect.X;
            int y = rect.Y;
            float d = (float)rect.Width / 6;
            float b = (float)rect.Height / 6;

            g.TranslateTransform(x + 3 * d, y + 3 * b);
            g.RotateTransform(Angle);
            g.TranslateTransform(-x - 3 * d, -y - 3 * b);

            g.DrawEllipse(pen, new RectangleF(x + 1 * d, y + (float)(0.5 * d), 3 * d, 3 * d));
            g.DrawEllipse(pen, new RectangleF(x + 2 * d, y + (float)(0.5 * d), 3 * d, 3 * d));

            g.DrawLine(pen, new PointF(x + 0 * d, y + 2 * d), new PointF(x + 1 * d, y + 2 * d));
            g.DrawLine(pen, new PointF(x + 5 * d, y + 2 * d), new PointF(x + 6 * d, y + 2 * d));
            g.ResetTransform();
            pen.Dispose();
            //ÐèÒªÑ§ÉúÌîÐ´
            //DrawName(g);
            Pen pen1 = new Pen(Color.Red, 1);
            Point p;
            switch (Node)
            {
                case 1:
                    p = GetNode(1);
                    g.DrawArc(pen1, new RectangleF(p.X - 5, p.Y - 5, 10, 10), 0, 360);
                    break;
                case 2:
                    p = GetNode(2);
                    g.DrawArc(pen1, new RectangleF(p.X - 5, p.Y - 5, 10, 10), 0, 360);
                    break;
            }
            pen1.Dispose();
        }
    }
}
