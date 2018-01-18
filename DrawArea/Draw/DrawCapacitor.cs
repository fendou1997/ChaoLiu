using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DrawArea
{
    [Serializable]
    public class DrawCapacitor : DrawRectanlge
    {
        public DrawCapacitor(int x, int y, int width, int height)
            : base(x, y, width, height)
        {
            base.Name = "电容器";
        }
        public DrawCapacitor()
            : this(0, 0, 120, 80)
        {
            base.Name = "电容器";
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
            Pen pen = new Pen(base.Color, base.PenWidth);

            Rectangle rect = DrawRectanlge.GetNormalizedRectangle(base.rectangle);
            int x = rect.X;
            int y = rect.Y;
            float d = (float)rect.Width / 6;
            float b = (float)rect.Height / 6;

            g.TranslateTransform(x + 3 * d, y + 3 * b);
            g.RotateTransform(Angle);
            g.TranslateTransform(-x - 3 * d, -y - 3 * b);

            g.DrawLine(pen, new PointF(x, y + 3 * b ), new PointF(x + 2 * d, y + 3 * b ));
            g.DrawLine(pen, new PointF(x + 2 * d, y + 1 * b ), new PointF(x + 2 * d, y + 5 * b ));
            g.DrawLine(pen, new PointF(x + 4 * d, y + 1 * b ), new PointF(x + 4 * d, y + 5 * b ));
            g.DrawLine(pen, new PointF(x + 4 * d, y + 3 * b ), new PointF(x + 6 * d, y + 3 * b ));
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
            }
            pen1.Dispose();
        }
    }
}
