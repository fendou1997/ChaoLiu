using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace DrawArea
{
    public class ToolPointer : ToolObject
    {
        #region 属性部分
        private PointerMode pointerMode;
        private Point startPoint;
        private Point lastPoint;
        /// <summary>
        /// 记录要改变尺寸的图元
        /// </summary>
        private DrawObject resizedObject;
        /// <summary>
        /// 记录锚点标识号
        /// </summary>
        private int resizedObjectHandle;
        private ComFunction CF = new ComFunction();
        #endregion

        public override void OnMouseDown(DrawArea drawArea, System.Windows.Forms.MouseEventArgs e)
        {
            this.pointerMode = PointerMode.None;
            Point point = new Point(e.X, e.Y);

            //检测指针是否能够处于“改变图元尺寸”的模式（图元被选择并且在锚点内按下鼠标）
            if (drawArea.GraphicsList.Selection != null)
            {
                foreach (DrawObject o in drawArea.GraphicsList.Selection)
                {
                    int handleNumber = o.HitTest(point);//判断点与图元之间的位置关系
                    if (handleNumber > 0)//点在锚点上
                    {
                        pointerMode = PointerMode.Size;//改变图形大小
                        this.resizedObject = o;
                        this.resizedObjectHandle = handleNumber;
                        drawArea.GraphicsList.UnselectAll();
                        o.Selected = true;
                        break;
                    }
                }
            }
            //检测指针是否能够处于“移动图元”的模式（鼠标在图元内部按下）
            if (pointerMode == PointerMode.None)
            {
                DrawObject o = null;
                for (int i = 0; i < drawArea.GraphicsList.Count; i++)
                {
                    if (drawArea.GraphicsList[i].HitTest(point) == 0)
                    {
                        o = drawArea.GraphicsList[i];//点击在图元内部
                        break;
                    }
                }
                if (o != null)
                {
                    pointerMode = PointerMode.Move;//移动图形
                    //没按Control键 并且 该图元没被选
                    if ((Control.ModifierKeys & Keys.Control) == 0 && o.Selected == false)
                        drawArea.GraphicsList.UnselectAll();

                    //选择被点击的图元
                    o.Selected = true;
                    drawArea.Cursor = Cursors.SizeAll;
                }
                //检测指针是否能够处于“净”的模式（鼠标在图元外部按下）
                if (pointerMode == PointerMode.None)
                {
                    if ((Control.ModifierKeys & Keys.Control) == 0)
                        drawArea.GraphicsList.UnselectAll();

                    pointerMode = PointerMode.Net;
                }
                lastPoint.X = e.X;
                lastPoint.Y = e.Y;
                startPoint.X = e.X;
                startPoint.Y = e.Y;
                drawArea.Capture = true;
                drawArea.Refresh();
                //净模式下绘制可逆线
                if (pointerMode == PointerMode.Net)
                {
                    ControlPaint.DrawReversibleFrame(drawArea.RectangleToScreen(DrawRectanlge.GetNormalizedRectangle(startPoint, lastPoint)),
                                            Color.Black, FrameStyle.Dashed);
                }
            }
        }

        public override void OnMouseMove(DrawArea drawArea, System.Windows.Forms.MouseEventArgs e)
        {
            Point point = new Point(e.X, e.Y);
            Point oldPoint = lastPoint;
            CF.NodeRange(drawArea, e);//画结点区域
            if (e.Button == MouseButtons.None)
            {
                #region 在没有按住任何鼠标键的情况下，随着鼠标移动而改变指针形状
                Cursor cursor = null;
                for (int i = 0; i < drawArea.GraphicsList.Count; i++)
                {
                    int n = drawArea.GraphicsList[i].HitTest(point);

                    if (n > 0)
                    {
                        cursor = drawArea.GraphicsList[i].GetHandleCursor(n);
                        break;
                    }
                }
                if (cursor == null)
                    cursor = Cursors.Default;
                drawArea.Cursor = cursor;
                return;
                #endregion
            }
            //按住的不是鼠标左键
            if (e.Button != MouseButtons.Left)
                return;

            //按住鼠标左键移动
            int dx = e.X - lastPoint.X;
            int dy = e.Y - lastPoint.Y;

            lastPoint.X = e.X;
            lastPoint.Y = e.Y;

            if (pointerMode == PointerMode.Size)
            {
                #region 改变大小
                if (resizedObject != null)
                {
                    if (resizedObject is DrawLine)
                    {
                        // 如果线的一端在结点范围之内则将该点与结点相连
                        Point p = CF.OnRange(drawArea);
                        if (p.X < 0 || p.Y < 0)
                        {
                            if (IsCtrlOn)
                            {
                                resizedObject.MoveHandleTo(point, 10 * resizedObjectHandle);
                            }
                            else if (IsShiftOn)
                            {
                                resizedObject.MoveHandleTo(point, 30 * resizedObjectHandle);
                            }
                            else
                            {
                                resizedObject.MoveHandleTo(point, resizedObjectHandle);
                            }
                        }
                        else
                        {
                            if (IsCtrlOn)
                            {
                                resizedObject.MoveHandleTo(p, 10 * resizedObjectHandle);
                            }
                            else if (IsShiftOn)
                            {
                                resizedObject.MoveHandleTo(p, 30 * resizedObjectHandle);
                            }
                            else
                            {
                                resizedObject.MoveHandleTo(p, resizedObjectHandle);
                            }
                        }
                    }
                    else
                    {
                        //如果移动的是元器件的结点
                        List<int[]> LIndex = CF.FindLineHandle(drawArea, resizedObject);
                        Point location;
                        if (IsShiftOn)
                        {
                            //如果该元器件有导线连接则导线随元器件的大小变动，此时是按住SHIFT键的时候
                            if (LIndex.Count != 0)
                            {
                                int[] index;
                                //循环遍历与该结点相连的所有直线锚点
                                for (int i = 0; i < LIndex.Count; i++)
                                {
                                    index = LIndex[i];//得到其中一个直线锚点
                                    location = resizedObject.GetNode(index[0]);
                                    resizedObject.MoveHandleTo(point, 10);

                                    Point newp = resizedObject.GetNode(index[0]);
                                    int cx = newp.X - location.X;
                                    int cy = newp.Y - location.Y;
                                    //找出相对应的直线，改变直线中相应锚点的位置
                                    DrawObject O = drawArea.GraphicsList[index[1]];
                                    O.MoveHandleTo(new Point(location.X + cx, location.Y + cy), index[2]);
                                }
                            }
                            else
                            {
                                resizedObject.MoveHandleTo(point, 10);
                            }
                        }
                        else
                        {
                            //如果该元器件有导线连接则导线随元器件的大小变动
                            if (LIndex.Count != 0)
                            {
                                int[] index;
                                for (int i = 0; i < LIndex.Count; i++)
                                {
                                    //得到结点连接的每一个直线锚点信息
                                    index = LIndex[i];

                                    location = resizedObject.GetNode(index[0]);
                                    resizedObject.MoveHandleTo(point, resizedObjectHandle);

                                    Point newp = resizedObject.GetNode(index[0]);
                                    int cx = newp.X - location.X;
                                    int cy = newp.Y - location.Y;
                                    DrawObject O = drawArea.GraphicsList[index[1]];
                                    O.MoveHandleTo(new Point(location.X + cx, location.Y + cy), index[2]);
                                }
                            }
                            else
                            {
                                resizedObject.MoveHandleTo(point, resizedObjectHandle);
                            }
                        }
                    }
                    drawArea.Refresh();
                }
                #endregion
            }

            if (pointerMode == PointerMode.Move)
            {
                #region 移动图形
                if (drawArea.GraphicsList.Selection != null)
                {
                    List<int[]> L;
                    foreach (DrawObject o in drawArea.GraphicsList.Selection)
                    {
                        Point location = new Point();
                        //找出与元器件o结点相连的直线描点
                        L = CF.FindLineHandle(drawArea, o);

                        int[] index;
                        for (int i = 0; i < L.Count; i++)
                        {
                            index = L[i];
                            location = o.GetNode(index[0]);
                            DrawObject O = drawArea.GraphicsList[index[1]];
                            O.MoveHandleTo(new Point(location.X + dx, location.Y + dy), index[2]);
                        }
                        o.Move(dx, dy);
                    }
                    drawArea.Cursor = Cursors.SizeAll;
                    drawArea.Refresh();
                }
                #endregion
            }

            if (pointerMode == PointerMode.Net)
            {
                #region 空绘制矩形
                // 移除旧的矩形
                ControlPaint.DrawReversibleFrame(
                    drawArea.RectangleToScreen(DrawRectanlge.GetNormalizedRectangle(startPoint, oldPoint)),
                    Color.Black,
                    FrameStyle.Dashed);
                // 绘制新的矩形
                ControlPaint.DrawReversibleFrame(
                    drawArea.RectangleToScreen(DrawRectanlge.GetNormalizedRectangle(startPoint, point)),
                    Color.Black,
                    FrameStyle.Dashed);
                #endregion
            }
        }

        public override void OnMouseUp(DrawArea drawArea, System.Windows.Forms.MouseEventArgs e)
        {
            if (drawArea.GraphicsList.Selection != null)
            {
                for (int i = 0; i < drawArea.GraphicsList.Selection.Count; i++)
                {
                    if (drawArea.GraphicsList.Selection[i] is DrawLine)
                    {
                        //如果线发生改变则需要判断线两端连接的结点状态以及重新进行深度优先搜索
                        DrawLine drawLine = drawArea.GraphicsList.Selection[i] as DrawLine;
                        CF.ChangeLineLink(drawArea, drawLine, e);

                    }
                    else if (drawArea.GraphicsList.Selection[i] is DrawBreak)
                    {
                        //如果是开关
                        DrawBreak drawBreak = drawArea.GraphicsList.Selection[i] as DrawBreak;
                        //根据开关开闭状态给开关对应的结点加边
                        if (drawBreak.OpenOrClose == true)
                        {
                            //给开关两端的首结点连线
                            CF.AddEdge(drawArea, drawBreak);
                        }
                        else
                        {
                            //断开开关两端的首结点连线
                            CF.ReduceEdge(drawArea, drawBreak);
                        }
                    }
                    else if (drawArea.GraphicsList.Selection[i] is DrawKnife)
                    {
                        //如果是刀闸
                        DrawKnife drawKnife = drawArea.GraphicsList.Selection[i] as DrawKnife;
                        //根据开关开闭状态给开关对应的结点加边
                        if (drawKnife.OpenOrClose == true)
                        {
                            //给开关两端的首结点连线
                            CF.AddEdge(drawArea, drawKnife);
                        }
                        else
                        {
                            //断开开关两端的首结点连线
                            CF.ReduceEdge(drawArea, drawKnife);
                        }
                    }
                }
            }
            if (drawArea.IsTest == true)
            {
                //进行测试
                CF.BeginTest(drawArea);
            }


            //空画矩形
            if (pointerMode == PointerMode.Net)
            {
                // 移除旧的矩形
                ControlPaint.DrawReversibleFrame(
                    drawArea.RectangleToScreen(DrawRectanlge.GetNormalizedRectangle(startPoint, lastPoint)),
                    Color.Black,
                    FrameStyle.Dashed);

                // 选择在矩形框中的图元
                drawArea.GraphicsList.SelectInRectangle(DrawRectanlge.GetNormalizedRectangle(startPoint, lastPoint));
                pointerMode = PointerMode.None;
            }

            if (resizedObject != null)
            {
                //改变大小之后
                resizedObject.Normalize();
                resizedObject = null;
            }
            drawArea.Capture = false;
            drawArea.Refresh();
        }
    }
}
