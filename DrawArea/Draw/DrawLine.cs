using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace DrawArea
{
    [Serializable]
    public class DrawLine : DrawObject
    {
        #region 属性部分
        [NonSerialized]
        protected GraphicsPath areaPath;
        [NonSerialized]
        protected Pen areaPen;
        [NonSerialized]
        protected Region areaRegion;
        /// <summary>
        /// 起点
        /// </summary>
        public Point startPoint;
        /// <summary>
        /// 终点
        /// </summary>
        public Point endPoint;
        /// <summary>
        /// 获取锚点个数
        /// </summary>
        public override int HandleCount
        {
            get
            {
                return 2;
            }
        }
        public int[] FindEquipmentID = new int[2];//直线锚点对应元器件左右
        public int[] FindNodeID = new int[2];//线连接的结点号
        #endregion

        #region 构造函数
        public DrawLine(int x1, int y1, int x2, int y2)
            : base()
        {
            base.Name = "母线";
            areaPath = default(GraphicsPath);
            areaPen = default(Pen);
            areaRegion = default(Region);
            startPoint.X = x1;
            startPoint.Y = y1;
            endPoint.X = x2;
            endPoint.Y = y2;
        }
        public DrawLine()
            : this(0, 0, 1, 1)
        {
            base.Name = "母线";
        }
        #endregion

        #region 与图形操作有关的方法
        /// <summary>
        /// 释放缓冲区
        /// </summary>
        protected void Invalidate()
        {
            if (areaPath != null)
            {
                areaPath.Dispose();
                areaPath = null;
            }

            if (areaPen != null)
            {
                areaPen.Dispose();
                areaPen = null;
            }

            if (areaRegion != null)
            {
                areaRegion.Dispose();
                areaRegion = null;
            }
        }
        /// <summary>
        /// 绘制母线
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g)
        {
            base.Draw(g);
            base.PenWidth = 2;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Pen pen = new Pen(base.Color, PenWidth);
            g.DrawLine(pen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
            pen.Dispose();
        }
        public override void Move(int deltaX, int deltaY)
        {
            startPoint.X += deltaX;
            startPoint.Y += deltaY;
            endPoint.X += deltaX;
            endPoint.Y += deltaY;
            this.Invalidate();
        }
        #endregion

        #region 与锚点有关的方法
        /// <summary>
        /// 根据编号得到锚点坐标
        /// </summary>
        /// <param name="handleNumber">对于直线:起点1,终点2.</param>
        /// <returns>图形锚点坐标</returns>
        public override Point GetHandle(int handleNumber)
        {
            if (handleNumber == 1)
                return startPoint;
            else
                return endPoint;
        }
        /// <summary>
        /// 根据编号得到锚点处的光标
        /// </summary>
        /// <param name="handleNumber">对于直线:起点1,终点2.</param>
        /// <returns>锚点处光标</returns>
        public override Cursor GetHandleCursor(int handleNumber)
        {
            switch (handleNumber)
            {
                case 1:
                case 2:
                    return Cursors.SizeAll;
                default:
                    return Cursors.Default;
            }
        }
        /// <summary>
        /// 根据锚点标号和传过来的点来调整坐标位置
        /// </summary>
        /// <param name="point"></param>
        /// <param name="handleNumber">矩形:左上角为1,右上角为2,右下角为3,左下角为4</param>
        public override void MoveHandleTo(Point point, int handleNumber)
        {
            if (handleNumber == 1)//起点移动
                startPoint = point;
            else if (handleNumber == 2)//终点移动
                endPoint = point;
            else if (handleNumber == 10)//下边线水平移动
            {
                startPoint.X = point.X;
                startPoint.Y = endPoint.Y;
            }
            else if (handleNumber == 20)//上边线水平移动
            {
                endPoint.X = point.X;
                endPoint.Y = startPoint.Y;
            }
            else if (handleNumber == 60)//左边线垂直移动
            {
                endPoint.X = startPoint.X;
                endPoint.Y = point.Y;
            }
            else//右边线垂直移动
            {
                startPoint.X = endPoint.X;
                startPoint.Y = point.Y;
            }
            this.Invalidate();
        }
        #endregion

        #region 判断与点的位置有关的方法
        /// <summary>
        /// 对给定的点进行测试
        /// 返回+1 该点左上角锚点内
        /// 返回+2 该点右上角锚点内
        /// 返回0  该点在矩形内部
        /// 返回-1 该点没在区域内部也没在锚点区域内部
        /// </summary>
        /// <param name="point">进行测试点的坐标</param>
        /// <returns></returns>
        public override int HitTest(Point point)
        {
            if (Selected)
            {
                for (int i = 1; i <= HandleCount; i++)
                {
                    if (GetHandleRectangle(i).Contains(point))
                        return i;
                }
            }
            if (PointInObject(point))
                return 0;
            return -1;
        }
        /// <summary>
        /// 创建直线的缓冲带
        /// </summary>
        protected virtual void CreateObjects()
        {
            if (this.areaPath != null)
                return;
            //创建含有多条直线的路径，以便于鼠标的选择
            this.areaPath = new GraphicsPath();
            this.areaPen = new Pen(Color.Black, 7);
            this.areaPath.AddLine(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
            this.areaPath.Widen(areaPen);
            // 创建直线的缓冲带
            this.areaRegion = new Region(areaPath);
        }
        /// <summary>
        /// 判断给定点是否在直线的区域内
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        protected override bool PointInObject(Point point)
        {
            this.CreateObjects();
            return areaRegion.IsVisible(point);
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
            this.CreateObjects();
            return areaRegion.IsVisible(rectangle);
        }
        #endregion

        public override int NodeCount
        {
            get
            {
                return 0;
            }
        }
        public override Point GetNode(int NodeNumber)
        {
            return new Point(0, 0);   //不调用该函数
        }

        public int ConncetSNodeCount()//该线连接的结点数
        {
            int count = 0;
            if (FindNodeID[0] != 0)
            {
                count++;
            }
            if (FindNodeID[1] != 0)
            {
                count++;
            }
            return count;
        }
    }
}
