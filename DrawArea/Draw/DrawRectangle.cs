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
        #region 属性部分
        protected Rectangle rectangle;
        /// <summary>
        /// 获取锚点个数
        /// </summary>
        public override int HandleCount
        {
            get 
            {
                return 4; 
            }
        }
        #endregion

        #region 构造函数
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

        #region 一般方法
        /// <summary>
        /// 移动图形
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        public override void Move(int deltaX, int deltaY)
        {
            rectangle.X += deltaX;
            rectangle.Y += deltaY;
        }
        /// <summary>
        /// 规范化图形
        /// </summary>
        public override void Normalize()
        {
            rectangle = GetNormalizedRectangle(rectangle);
        }
        /// <summary>
        /// 规范化图形
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static Rectangle GetNormalizedRectangle(Rectangle r)
        {
            return GetNormalizedRectangle(r.X, r.Y, r.X + r.Width, r.Y + r.Height);
        }
        /// <summary>
        /// 规范化图形
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static Rectangle GetNormalizedRectangle(Point p1, Point p2)
        {
            return GetNormalizedRectangle(p1.X, p1.Y, p2.X, p2.Y);
        }
        /// <summary>
        /// 规范化图形
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
        /// 绘制设备名称
        /// </summary>
        /// <param name="g"></param>
        public override void DrawName(Graphics g)
        {
            int fontsize = rectangle.Width / 10;
            Font Font;
            if (fontsize < 2)
            {
                Font = new Font("宋体", 3);
            }
            else
            {
                Font = new Font("宋体", fontsize);
            }
            StringFormat strFormat = new StringFormat();
            int d = rectangle.Width / 6;
            int b = rectangle.Height / 6;
            SolidBrush brush = new SolidBrush(Color.Black);
            g.DrawString(Name, Font, brush, rectangle.X + d, rectangle.Y + rectangle.Height + b, strFormat);
        }
        #region 与锚点有关的方法
        /// <summary>
        /// 根据编号得到锚点坐标
        /// </summary>
        /// <param name="handleNumber">左上角为1,右上角为2,右下角为3,左下角为4</param>
        /// <returns>图形锚点坐标</returns>
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
        /// 根据编号得到锚点处的光标
        /// </summary>
        /// <param name="handleNumber">左上角为1,右上角为2,右下角为3,左下角为4</param>
        /// <returns>锚点处光标</returns>
        public override Cursor GetHandleCursor(int handleNumber)
        {
            switch (handleNumber)
            {
                case 1:
                    return Cursors.SizeNWSE;//左上角
                case 2:
                    return Cursors.SizeNESW;//右上角
                case 3:
                    return Cursors.SizeNWSE;//右下角
                case 4:
                    return Cursors.SizeNESW;//左下角
                default:
                    return Cursors.Default;
            }
        }
        /// <summary>
        /// 根据锚点标号和传过来的点来调整坐标位置
        /// </summary>
        /// <param name="point"></param>
        /// <param name="handleNumber">左上角为1,右上角为2,右下角为3,左下角为4</param>
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

        #region 判断与点的位置有关的方法
        /// <summary>
        /// 对给定的点进行测试
        /// 返回+1 该点左上角锚点内
        /// 返回+2 该点右上角锚点内
        /// 返回+3 该点右下角锚点内
        /// 返回+4 该点左下角锚点内
        /// 返回0  该点在矩形内部
        /// 返回-1 该点没在区域内部也没在锚点区域内部
        /// </summary>
        /// <param name="point">进行测试点的坐标</param>
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
        /// 判断给定点是否在图形内
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        protected override bool PointInObject(Point point)
        {
            return rectangle.Contains(point);
        }
        #endregion

        #region 判断与矩形位置有关的方法
        /// <summary>
        /// 判断图元与所给矩形是否存在交集
        /// </summary>
        /// <param name="rectangle">测试的矩形</param>
        /// <returns></returns>
        public override bool IntersectsWith(Rectangle rectangle)
        {
            return this.rectangle.IntersectsWith(rectangle);
        }
        #endregion
        #endregion
    }
}
