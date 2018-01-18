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
            base.Name = "����";
        }
        public DrawBreak()
            : this(0, 0, 80, 40)
        {
            base.Name = "����";
        }

        private bool openOrclose = false;
        /// <summary>
        /// ��ʾ����״̬
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
        /// ���ز���
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
        public override int NodeCount//��ȡ�����
        {
            get
            {
                return 2;
            }
        }
        public override Point GetNode(int NodeNumber)//��ȡ�������
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
        /// ��ͼ����
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
            //��Ҫѧ����д
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