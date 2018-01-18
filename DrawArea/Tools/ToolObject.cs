using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DrawArea
{
    public abstract class ToolObject
    {
        #region 属性部分
        /// <summary>
        /// 获取或设置工具的光标
        /// </summary>
        protected Cursor Cursor;
        /// <summary>
        /// 获取或设置是否按下了Ctrl键
        /// </summary>
        public bool IsCtrlOn = default(bool);
        /// <summary>
        /// 获取或设置是否按下了Shift键
        /// </summary>
        public bool IsShiftOn = default(bool);
        #endregion

        #region 方法部分
        /// <summary>
        /// 按下鼠标左键
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public abstract void OnMouseDown(DrawArea drawArea, MouseEventArgs e);
        /// <summary>
        /// 移动鼠标（鼠标左键被按下或者没有按任何鼠标键）
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public abstract void OnMouseMove(DrawArea drawArea, MouseEventArgs e);
        /// <summary>
        /// 鼠标左键被释放
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public abstract void OnMouseUp(DrawArea drawArea, MouseEventArgs e);
        /// <summary>
        /// 把该图形添加到绘图区域中
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="o"></param>
        protected void AddNewObject(DrawArea drawArea, DrawObject obj)
        {
            drawArea.GraphicsList.UnselectAll();

            obj.Selected = true;
            drawArea.GraphicsList.Add(obj);

            drawArea.Capture = true;
            drawArea.Refresh();
        }
        /// <summary>
        /// 把该结点添加到绘图区域中
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="Ver"></param>
        protected void AddNewNode(DrawArea drawArea, VertexNode Ver)
        {
            drawArea.GraphicsNodeList.Add(Ver);
            drawArea.Refresh();
        }
        #endregion

    }
}
