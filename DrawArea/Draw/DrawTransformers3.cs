using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DrawArea
{
    [Serializable]
    public class DrawTransformer3 : DrawRectanlge
    {
        public DrawTransformer3(int x, int y, int width, int height)
            : base(x, y, width, height)
        {
            base.Name = "三绕变压器";
        }

        public DrawTransformer3()
            : this(0, 0, 120, 80)
        {
            base.Name = "三绕变压器";
        }
        public override int NodeCount
        {
            get
            {
                return 3;
            }
        }
        public override Point GetNode(int NodeNumber)
        {
            int x = rectangle.Left;
            int y = rectangle.Top;
            float b = (float)(rectangle.Height / 4.0);
            switch (NodeNumber)
            {
                case 1:
                    x = rectangle.Left;
                    y = rectangle.Top + (int)(3 * b);
                    break;
                case 2:
                    x = rectangle.Right;
                    y = rectangle.Top + (int)(3 * b);
                    break;
                case 3:
                    x = rectangle.Left + (rectangle.Width / 2);
                    y = rectangle.Top;
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
            float b = (float)rect.Height / 4;

            g.TranslateTransform(x + 3 * d, y + 3 * b);
            g.RotateTransform(Angle);
            g.TranslateTransform(-x - 3 * d, -y - 3 * b);

            g.DrawEllipse(pen, new RectangleF(x + 2 * d, y + 1 * b, 2 * d, 2 * d));
            g.DrawEllipse(pen, new RectangleF(x + (float)1.3 * d, y + 2 * b, 2 * d, 2 * d));
            g.DrawEllipse(pen, new RectangleF(x + (float)2.7 * d, y + 2 * b, 2 * d, 2 * d));

            g.DrawLine(pen, new PointF(x + 3 * d, y + 0 * b), new PointF(x + 3 * d, y + 1 * b));
            g.DrawLine(pen, new PointF(x + 0 * d, y + 3 * b), new PointF(x + (float)1.3 * d, y + 3 * b));
            g.DrawLine(pen, new PointF(x + (float)4.7 * d, y + 3 * b), new PointF(x + 6 * d, y + 3 * b));
            g.ResetTransform();
            pen.Dispose();
            //需要学生填写
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
                case 3:
                    p = GetNode(3);
                    g.DrawArc(pen1, new RectangleF(p.X - 5, p.Y - 5, 10, 10), 0, 360);
                    break;
            }
            pen1.Dispose();
        }
    }
}
