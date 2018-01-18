using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace DrawArea
{
    public class ToolPointer : ToolObject
    {
        #region ���Բ���
        private PointerMode pointerMode;
        private Point startPoint;
        private Point lastPoint;
        /// <summary>
        /// ��¼Ҫ�ı�ߴ��ͼԪ
        /// </summary>
        private DrawObject resizedObject;
        /// <summary>
        /// ��¼ê���ʶ��
        /// </summary>
        private int resizedObjectHandle;
        private ComFunction CF = new ComFunction();
        #endregion

        public override void OnMouseDown(DrawArea drawArea, System.Windows.Forms.MouseEventArgs e)
        {
            this.pointerMode = PointerMode.None;
            Point point = new Point(e.X, e.Y);

            //���ָ���Ƿ��ܹ����ڡ��ı�ͼԪ�ߴ硱��ģʽ��ͼԪ��ѡ������ê���ڰ�����꣩
            if (drawArea.GraphicsList.Selection != null)
            {
                foreach (DrawObject o in drawArea.GraphicsList.Selection)
                {
                    int handleNumber = o.HitTest(point);//�жϵ���ͼԪ֮���λ�ù�ϵ
                    if (handleNumber > 0)//����ê����
                    {
                        pointerMode = PointerMode.Size;//�ı�ͼ�δ�С
                        this.resizedObject = o;
                        this.resizedObjectHandle = handleNumber;
                        drawArea.GraphicsList.UnselectAll();
                        o.Selected = true;
                        break;
                    }
                }
            }
            //���ָ���Ƿ��ܹ����ڡ��ƶ�ͼԪ����ģʽ�������ͼԪ�ڲ����£�
            if (pointerMode == PointerMode.None)
            {
                DrawObject o = null;
                for (int i = 0; i < drawArea.GraphicsList.Count; i++)
                {
                    if (drawArea.GraphicsList[i].HitTest(point) == 0)
                    {
                        o = drawArea.GraphicsList[i];//�����ͼԪ�ڲ�
                        break;
                    }
                }
                if (o != null)
                {
                    pointerMode = PointerMode.Move;//�ƶ�ͼ��
                    //û��Control�� ���� ��ͼԪû��ѡ
                    if ((Control.ModifierKeys & Keys.Control) == 0 && o.Selected == false)
                        drawArea.GraphicsList.UnselectAll();

                    //ѡ�񱻵����ͼԪ
                    o.Selected = true;
                    drawArea.Cursor = Cursors.SizeAll;
                }
                //���ָ���Ƿ��ܹ����ڡ�������ģʽ�������ͼԪ�ⲿ���£�
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
                //��ģʽ�»��ƿ�����
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
            CF.NodeRange(drawArea, e);//���������
            if (e.Button == MouseButtons.None)
            {
                #region ��û�а�ס�κ�����������£���������ƶ����ı�ָ����״
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
            //��ס�Ĳ���������
            if (e.Button != MouseButtons.Left)
                return;

            //��ס�������ƶ�
            int dx = e.X - lastPoint.X;
            int dy = e.Y - lastPoint.Y;

            lastPoint.X = e.X;
            lastPoint.Y = e.Y;

            if (pointerMode == PointerMode.Size)
            {
                #region �ı��С
                if (resizedObject != null)
                {
                    if (resizedObject is DrawLine)
                    {
                        // ����ߵ�һ���ڽ�㷶Χ֮���򽫸õ���������
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
                        //����ƶ�����Ԫ�����Ľ��
                        List<int[]> LIndex = CF.FindLineHandle(drawArea, resizedObject);
                        Point location;
                        if (IsShiftOn)
                        {
                            //�����Ԫ�����е�������������Ԫ�����Ĵ�С�䶯����ʱ�ǰ�סSHIFT����ʱ��
                            if (LIndex.Count != 0)
                            {
                                int[] index;
                                //ѭ��������ý������������ֱ��ê��
                                for (int i = 0; i < LIndex.Count; i++)
                                {
                                    index = LIndex[i];//�õ�����һ��ֱ��ê��
                                    location = resizedObject.GetNode(index[0]);
                                    resizedObject.MoveHandleTo(point, 10);

                                    Point newp = resizedObject.GetNode(index[0]);
                                    int cx = newp.X - location.X;
                                    int cy = newp.Y - location.Y;
                                    //�ҳ����Ӧ��ֱ�ߣ��ı�ֱ������Ӧê���λ��
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
                            //�����Ԫ�����е�������������Ԫ�����Ĵ�С�䶯
                            if (LIndex.Count != 0)
                            {
                                int[] index;
                                for (int i = 0; i < LIndex.Count; i++)
                                {
                                    //�õ�������ӵ�ÿһ��ֱ��ê����Ϣ
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
                #region �ƶ�ͼ��
                if (drawArea.GraphicsList.Selection != null)
                {
                    List<int[]> L;
                    foreach (DrawObject o in drawArea.GraphicsList.Selection)
                    {
                        Point location = new Point();
                        //�ҳ���Ԫ����o���������ֱ�����
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
                #region �ջ��ƾ���
                // �Ƴ��ɵľ���
                ControlPaint.DrawReversibleFrame(
                    drawArea.RectangleToScreen(DrawRectanlge.GetNormalizedRectangle(startPoint, oldPoint)),
                    Color.Black,
                    FrameStyle.Dashed);
                // �����µľ���
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
                        //����߷����ı�����Ҫ�ж����������ӵĽ��״̬�Լ����½��������������
                        DrawLine drawLine = drawArea.GraphicsList.Selection[i] as DrawLine;
                        CF.ChangeLineLink(drawArea, drawLine, e);

                    }
                    else if (drawArea.GraphicsList.Selection[i] is DrawBreak)
                    {
                        //����ǿ���
                        DrawBreak drawBreak = drawArea.GraphicsList.Selection[i] as DrawBreak;
                        //���ݿ��ؿ���״̬�����ض�Ӧ�Ľ��ӱ�
                        if (drawBreak.OpenOrClose == true)
                        {
                            //���������˵��׽������
                            CF.AddEdge(drawArea, drawBreak);
                        }
                        else
                        {
                            //�Ͽ��������˵��׽������
                            CF.ReduceEdge(drawArea, drawBreak);
                        }
                    }
                    else if (drawArea.GraphicsList.Selection[i] is DrawKnife)
                    {
                        //����ǵ�բ
                        DrawKnife drawKnife = drawArea.GraphicsList.Selection[i] as DrawKnife;
                        //���ݿ��ؿ���״̬�����ض�Ӧ�Ľ��ӱ�
                        if (drawKnife.OpenOrClose == true)
                        {
                            //���������˵��׽������
                            CF.AddEdge(drawArea, drawKnife);
                        }
                        else
                        {
                            //�Ͽ��������˵��׽������
                            CF.ReduceEdge(drawArea, drawKnife);
                        }
                    }
                }
            }
            if (drawArea.IsTest == true)
            {
                //���в���
                CF.BeginTest(drawArea);
            }


            //�ջ�����
            if (pointerMode == PointerMode.Net)
            {
                // �Ƴ��ɵľ���
                ControlPaint.DrawReversibleFrame(
                    drawArea.RectangleToScreen(DrawRectanlge.GetNormalizedRectangle(startPoint, lastPoint)),
                    Color.Black,
                    FrameStyle.Dashed);

                // ѡ���ھ��ο��е�ͼԪ
                drawArea.GraphicsList.SelectInRectangle(DrawRectanlge.GetNormalizedRectangle(startPoint, lastPoint));
                pointerMode = PointerMode.None;
            }

            if (resizedObject != null)
            {
                //�ı��С֮��
                resizedObject.Normalize();
                resizedObject = null;
            }
            drawArea.Capture = false;
            drawArea.Refresh();
        }
    }
}
