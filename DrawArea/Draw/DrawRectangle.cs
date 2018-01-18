using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;


namespace DrawArea
{
    [Serializable]
    public abstract class DrawRectanlge : DrawObject
    {
        #region ���Բ���
        protected Rectangle rectangle;
        /// <summary>
        /// ��ȡê�����
        /// </summary>
        public override int HandleCount
        {
            get 
            {
                return 4; 
            }
        }
        #endregion

        #region ���캯��
        public DrawRectanlge(int x, int y, int width, int height)
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
        }

        public DrawRectanlge()
            : this(0, 0, 120, 80)
        {

        }
        #endregion

        #region һ�㷽��
        /// <summary>
        /// �ƶ�ͼ��
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        public override void Move(int deltaX, int deltaY)
        {
            rectangle.X += deltaX;
            rectangle.Y += deltaY;
        }
        /// <summary>
        /// �淶��ͼ��
        /// </summary>
        public override void Normalize()
        {
            rectangle = GetNormalizedRectangle(rectangle);
        }
        /// <summary>
        /// �淶��ͼ��
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static Rectangle GetNormalizedRectangle(Rectangle r)
        {
            return GetNormalizedRectangle(r.X, r.Y, r.X + r.Width, r.Y + r.Height);
        }
        /// <summary>
        /// �淶��ͼ��
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static Rectangle GetNormalizedRectangle(Point p1, Point p2)
        {
            return GetNormalizedRectangle(p1.X, p1.Y, p2.X, p2.Y);
        }
        /// <summary>
        /// �淶��ͼ��
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static Rectangle GetNormalizedRectangle(int x1, int y1, int x2, int y2)
        {
            if (x2 < x1)
            {
                int tmp = x2;
                x2 = x1;
                x1 = tmp;
            }

            if (y2 < y1)
            {
                int tmp = y2;
                y2 = y1;
                y1 = tmp;
            }

            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }
        /// <summary>
        /// �����豸����
        /// </summary>
        /// <param name="g"></param>
        public override void DrawName(Graphics g)
        {
            int fontsize = rectangle.Width / 10;
            Font Font;
            if (fontsize < 2)
            {
                Font = new Font("����", 3);
            }
            else
            {
                Font = new Font("����", fontsize);
            }
            StringFormat strFormat = new StringFormat();
            int d = rectangle.Width / 6;
            int b = rectangle.Height / 6;
            SolidBrush brush = new SolidBrush(Color.Black);
            g.DrawString(Name, Font, brush, rectangle.X + d, rectangle.Y + rectangle.Height + b, strFormat);
        }
        #region ��ê���йصķ���
        /// <summary>
        /// ���ݱ�ŵõ�ê������
        /// </summary>
        /// <param name="handleNumber">���Ͻ�Ϊ1,���Ͻ�Ϊ2,���½�Ϊ3,���½�Ϊ4</param>
        /// <returns>ͼ��ê������</returns>
        public override Point GetHandle(int handleNumber)
        {
            int x = rectangle.Left;
            int y = rectangle.Top;

            switch (handleNumber)
            {
                case 1:
                    x = rectangle.Left;
                    y = rectangle.Top;
                    break;
                case 2:
                    x = rectangle.Right;
                    y = rectangle.Top;
                    break;
                case 3:
                    x = rectangle.Right;
                    y = rectangle.Bottom;
                    break;
                case 4:
                    x = rectangle.Left;
                    y = rectangle.Bottom;
                    break;
            }
            return new Point(x, y);
        }
        /// <summary>
        /// ���ݱ�ŵõ�ê�㴦�Ĺ��
        /// </summary>
        /// <param name="handleNumber">���Ͻ�Ϊ1,���Ͻ�Ϊ2,���½�Ϊ3,���½�Ϊ4</param>
        /// <returns>ê�㴦���</returns>
        public override Cursor GetHandleCursor(int handleNumber)
        {
            switch (handleNumber)
            {
                case 1:
                    return Cursors.SizeNWSE;//���Ͻ�
                case 2:
                    return Cursors.SizeNESW;//���Ͻ�
                case 3:
                    return Cursors.SizeNWSE;//���½�
                case 4:
                    return Cursors.SizeNESW;//���½�
                default:
                    return Cursors.Default;
            }
        }
        /// <summary>
        /// ����ê���źʹ������ĵ�����������λ��
        /// </summary>
        /// <param name="point"></param>
        /// <param name="handleNumber">���Ͻ�Ϊ1,���Ͻ�Ϊ2,���½�Ϊ3,���½�Ϊ4</param>
        public override void MoveHandleTo(Point point, int handleNumber)
        {
            int left = rectangle.Left;
            int top = rectangle.Top;
            int right = rectangle.Right;
            int bottom = rectangle.Bottom;
            int width = right - left;
            int height = bottom - top;

            switch (handleNumber)
            {
                case 1:
                    left = point.X;
                    top = point.Y;
                    width = right - left;
                    height = width * 2 / 3;
                    top = bottom - height;
                    break;
                case 2:
                    right = point.X;
                    top = point.Y;
                    width = right - left;
                    height = width * 2 / 3;
                    top = bottom - height;
                    break;
                case 3:
                    right = point.X;
                    bottom = point.Y;
                    width = right - left;
                    height = width * 2 / 3;
                    break;
                case 4:
                    left = point.X;
                    bottom = point.Y;
                    width = right - left;
                    height = width * 2 / 3;
                    break;
            }

            rectangle.X = left;
            rectangle.Y = top;
            rectangle.Width = width;
            rectangle.Height = height;
        }
        #endregion

        #region �ж�����λ���йصķ���
        /// <summary>
        /// �Ը����ĵ���в���
        /// ����+1 �õ����Ͻ�ê����
        /// ����+2 �õ����Ͻ�ê����
        /// ����+3 �õ����½�ê����
        /// ����+4 �õ����½�ê����
        /// ����0  �õ��ھ����ڲ�
        /// ����-1 �õ�û�������ڲ�Ҳû��ê�������ڲ�
        /// </summary>
        /// <param name="point">���в��Ե������</param>
        /// <returns></returns>
        public override int HitTest(Point point)
        {
            if (base.Selected)
            {
                for (int i = 1; i <= HandleCount; i++)
                {
                    if (base.GetHandleRectangle(i).Contains(point))
                        return i;
                }
            }

            if (PointInObject(point))
                return 0;
            return -1;
        }
        /// <summary>
        /// �жϸ������Ƿ���ͼ����
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        protected override bool PointInObject(Point point)
        {
            return rectangle.Contains(point);
        }
        #endregion

        #region �ж������λ���йصķ���
        /// <summary>
        /// �ж�ͼԪ�����������Ƿ���ڽ���
        /// </summary>
        /// <param name="rectangle">���Եľ���</param>
        /// <returns></returns>
        public override bool IntersectsWith(Rectangle rectangle)
        {
            return this.rectangle.IntersectsWith(rectangle);
        }
        #endregion
        #endregion
    }
}
