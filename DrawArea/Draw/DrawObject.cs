using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace DrawArea
{
    [Serializable]
    public abstract class DrawObject
    {
        #region 属性部分
        /// <summary>
        /// 获取或设置最近一次绘图的颜色
        /// </summary>
        public static Color LastUsedColor = Color.Black;
        /// <summary>
        /// 获取或设置最后一次使用画笔的粗细
        /// </summary>
        public static int LastUsedPenWidth = 1;

        /// <summary>
        /// 获取或设置唯一标识设备的编号
        /// </summary>
        public int ObjectID = default(int);
        /// <summary>
        /// 获取或设置设备名称
        /// string.Empty表示string类型文本
        /// </summary>
        public string Name = string.Empty;
        /// <summary>
        /// 获取或设置标示设备是否带电
        /// </summary>
        public bool IsPowerOn = default(bool);
        /// <summary>
        /// 获取或设置图形的颜色
        /// </summary>
        public Color Color;
        /// <summary>
        /// 获取或设置画笔的粗细
        /// 使用default(int)，代替get,set
        /// </summary>
        public int PenWidth = default(int);
        /// <summary>
        /// 获取或设置图形是否被选择的标识
        /// </summary>
        public bool Selected = default(bool);
        /// <summary>
        /// 获取或设置设备旋转的角度
        /// </summary>
        public int Angle = 0;
        /// <summary>
        /// 获取或设置鼠标触碰的结点号，没有触碰时为-1
        /// </summary>
        public int Node = -1;
        /// <summary>
        /// 标志设备的开关状态
        /// </summary>
        public virtual bool OpenOrClose
        {
            get 
            {
                return true;
            }
            set
            {
                ;
            }
        }
        /// <summary>
        /// 获取锚点个数
        /// </summary>
        public abstract int HandleCount
        {
            get;
        }
        /// <summary>
        /// 获取结点个数
        /// </summary>
        public abstract int NodeCount
        {
            get;
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public DrawObject()
        {
            this.Color = DrawObject.LastUsedColor;
            this.PenWidth = DrawObject.LastUsedPenWidth;
        }
        #endregion

        #region 与图形操作有关的方法
        
        /// <summary>
        /// 绘制图形
        /// </summary>
        /// <param name="g"></param>
        public virtual void Draw(Graphics g)
        {
            if (this.IsPowerOn)
            {
                Color = System.Drawing.Color.Red;
            }
            else
            {
                Color = LastUsedColor;
            }
        }
        
        /// <summary>
        /// 移动图形
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        public abstract void Move(int deltaX, int deltaY);
        
        /// <summary>
        /// 规范化图形
        /// </summary>
        public virtual void Normalize()
        {
            ;
        }
        #endregion

        #region 与锚点有关的方法
        /// <summary>
        /// 根据编号得到锚点坐标
        /// </summary>
        /// <param name="handleNumber">对于矩形:左上角为1,右上角为2,右下角为3,左下角为4;对于直线:起点1,终点2.</param>
        /// <returns>图形锚点坐标</returns>
        public abstract Point GetHandle(int handleNumber);
        /// <summary>
        /// 根据锚点号得到绘制锚点的矩形
        /// </summary>
        /// <param name="handleNumber">左上角为1,右上角为2,右下角为3,左下角为4</param>
        /// <returns>图形锚点矩形</returns>
        public virtual Rectangle GetHandleRectangle(int handleNumber)
        {
            Point point = GetHandle(handleNumber);
            return new Rectangle(point.X - 3, point.Y - 3, 7, 7);
        }
        /// <summary>
        /// 根据编号得到锚点处的光标
        /// </summary>
        /// <param name="handleNumber">左上角为1,右上角为2,右下角为3,左下角为4</param>
        /// <returns>锚点处光标</returns>
        public abstract Cursor GetHandleCursor(int handleNumber);
        /// <summary>
        /// 根据锚点标号和传过来的点来调整坐标位置
        /// </summary>
        /// <param name="point"></param>
        /// <param name="handleNumber">矩形:左上角为1,右上角为2,右下角为3,左下角为4</param>
        public abstract void MoveHandleTo(Point point, int handleNumber);
        /// <summary>
        /// 为图形绘制锚点
        /// </summary>
        /// <param name="g"></param>
        public virtual void DrawTracker(Graphics g)
        {
            if (this.Selected == false) return;

            SolidBrush brush = new SolidBrush(Color.Black);
            for (int i = 1; i <= HandleCount; i++)
            {
                g.FillRectangle(brush, GetHandleRectangle(i));
            }
            brush.Dispose();
        }
        #endregion

        #region 判断与点的位置有关的方法
        /// <summary>
        /// 对给定的点进行测试
        /// </summary>
        /// <param name="point">进行测试点的坐标</param>
        /// <returns></returns>
        public abstract int HitTest(Point point);
        /// <summary>
        /// 判断给定点是否在图形内
        /// </summary>
        /// <param name="point">进行测试点的坐标</param>
        /// <returns></returns>
        protected abstract bool PointInObject(Point point);
        #endregion

        #region 判断与矩形位置有关的方法
        /// <summary>
        /// 判断图元与所给矩形是否存在交集
        /// </summary>
        /// <param name="rectangle">测试的矩形</param>
        /// <returns></returns>
        public abstract bool IntersectsWith(Rectangle rectangle);
        #endregion

        #region 与结点有关的方法
        /// <summary>
        /// 根据编号得到结点坐标
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public abstract Point GetNode(int NodeNumber);
        /// <summary>
        /// 绘制设备名称
        /// </summary>
        /// <param name="g"></param>
        public virtual void DrawName(Graphics g)
        {
        }
        
        #endregion

    }
}
