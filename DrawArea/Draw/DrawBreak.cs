using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;

namespace DrawArea
{
    [Serializable]
    public class DrawBreak : DrawRectanlge
    {
        public DrawBreak(int x, int y, int width, int height)
            : base(x, y, width, height)
        {
            base.Name = "开关";
        }
        public DrawBreak()
            : this(0, 0, 80, 40)
        {
            base.Name = "开关";
        }

        private bool openOrclose = false;
        /// <summary>
        /// 表示开关状态
        /// </summary>
        public override bool OpenOrClose
        {
            get
            {
                return openOrclose;
            }
            set
            {
                openOrclose = value;
            }
        }
        /// <summary>
        /// 开关操作
        /// </summary>
        public void Switch()
        {
            if (openOrclose == false)
            {
                openOrclose = true;
            }
            else
            {
                openOrclose = false;
            }
        }
        public override int NodeCount//获取结点数
        {
            get
            {
                return 2;
            }
        }
        public override Point GetNode(int NodeNumber)//获取结点坐标
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

        /// <summary>
        /// 画图方法
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(System.Drawing.Graphics g)
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
            
            g.DrawLine(pen, new PointF(x + 0 * d, y + 3 * b ), new PointF(x + 1 * d, y + 3 * b ));
            g.DrawLine(pen, new PointF(x + 1 * d, y + 2 * b ), new PointF(x + 1 * d, y + 4 * b ));
            g.DrawLine(pen, new PointF(x + 1 * d, y + 2 * b ), new PointF(x + 5 * d, y + 2 * b ));
            g.DrawLine(pen, new PointF(x + 1 * d, y + 4 * b ), new PointF(x + 5 * d, y + 4 * b ));
            g.DrawLine(pen, new PointF(x + 5 * d, y + 2 * b ), new PointF(x + 5 * d, y + 4 * b ));
            g.DrawLine(pen, new PointF(x + 5 * d, y + 3 * b ), new PointF(x + 6 * d, y + 3 * b ));
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
