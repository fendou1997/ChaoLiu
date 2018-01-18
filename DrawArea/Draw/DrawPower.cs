using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace DrawArea
{
    [Serializable]
    public class DrawPower : DrawRectanlge
    {
        public DrawPower(int x, int y, int wid, int hei)
            : base(x, y, wid, hei)
        {
            base.Name = "电源";
        }
        public DrawPower()
            : this(0, 0, 80, 40)
        {
            base.Name = "电源";
        }
        public override int NodeCount
        {
            get
            {
                return 1;
            }
        }
        public override Point GetNode(int NodeNumber)
        {
            int x = rectangle.Left;
            int y = rectangle.Top;
            if (NodeNumber == 1)
            {
                x = rectangle.Right;
                y = rectangle.Top + (int)(rectangle.Height / 2);
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

            g.DrawEllipse(pen, new RectangleF(x + 0 * d, y + 0 * b, 6 * b, 6 * b));
            g.DrawBezier(pen, new PointF(x + 0 * d, y + 3 * b), new PointF(x + 1 * d, y + 1 * b), new PointF(x + 3 * d, y + 5 * b), new PointF(x + 6 * b, y + 3 * b));

            g.DrawLine(pen, new PointF(x + 6 * b, y + 3 * b), new PointF(x + 6 * d, y + 3 * b));

            g.ResetTransform();
            pen.Dispose();
            //需要学生填写
            //DrawName(g);

            Pen pen1 = new Pen(Color.Red, 1);
            switch (Node)
            {
                case 1:
                    Point p = GetNode(1);
                    g.DrawArc(pen1, new RectangleF(p.X - 5, p.Y - 5, 10, 10), 0, 360);
                    break;
            }
            pen1.Dispose();
        }
    }
}
