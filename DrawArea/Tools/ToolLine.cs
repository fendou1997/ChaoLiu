using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DrawArea
{
    public class ToolLine : ToolObject
    {
        private ComFunction CF = new ComFunction();
        public ToolLine()
        {
            byte[] obj = global::DrawArea.Properties.Resources.Line;
            Cursor = new Cursor(new System.IO.MemoryStream(obj));
        }

        public override void OnMouseDown(DrawArea drawArea, System.Windows.Forms.MouseEventArgs e)
        {
            DrawLine dl;
            Point p = CF.OnRange(drawArea);
            if (p.X < 0 || p.Y < 0)
            {
                if (IsCtrlOn) //������Ctrl��
                {
                    dl = new DrawLine(e.X, e.Y, e.X + 10, e.Y);
                }
                else if (IsShiftOn)
                {
                    dl = new DrawLine(e.X, e.Y, e.X, e.Y + 10);
                }
                else
                {
                    dl = new DrawLine(e.X, e.Y, e.X + 10, e.Y + 10);
                }
                drawArea.Refresh();
            }
            else
            {
                if (IsCtrlOn) //������Ctrl��
                {
                    dl = new DrawLine(p.X, p.Y, p.X + 10, p.Y);
                }
                else if (IsShiftOn)
                {
                    dl = new DrawLine(p.X, p.Y, p.X, p.Y + 10);
                }
                else
                {
                    dl = new DrawLine(p.X, p.Y, p.X + 10, p.Y + 10);
                }
                drawArea.Refresh();
            }
            drawArea.Refresh();
            dl.ObjectID = ComFunction.NewEquipmentNumber ();
            AddNewObject(drawArea, dl);
            //��¼ֱ���������ӵ�Ԫ������źͽ����
            DrawLine d = (drawArea.GraphicsList[0]) as DrawLine;
            CF.ChangeLineLink(drawArea, d, e);
        }

        public override void OnMouseMove(DrawArea drawArea, System.Windows.Forms.MouseEventArgs e)
        {
            CF.NodeRange(drawArea, e);//���������
            drawArea.Cursor = Cursor;

            Point p = CF.OnRange(drawArea);
            if (p.X < 0 || p.Y < 0)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (IsCtrlOn)
                    {
                        Point point = new Point(e.X, e.Y);
                        drawArea.GraphicsList[0].MoveHandleTo(point, 20);
                    }
                    else if (IsShiftOn)
                    {
                        Point point = new Point(e.X, e.Y);
                        drawArea.GraphicsList[0].MoveHandleTo(point, 60);
                    }
                    else
                    {
                        Point point = new Point(e.X, e.Y);
                        drawArea.GraphicsList[0].MoveHandleTo(point, 2);
                    }
                    drawArea.Refresh();
                }
            }
            else
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (IsCtrlOn)
                    {
                        Point point = new Point(p.X, p.Y);
                        drawArea.GraphicsList[0].MoveHandleTo(point, 20);
                    }
                    else if (IsShiftOn)
                    {
                        Point point = new Point(p.X, p.Y);
                        drawArea.GraphicsList[0].MoveHandleTo(point, 60);
                    }
                    else
                    {
                        Point point = new Point(p.X, p.Y);
                        drawArea.GraphicsList[0].MoveHandleTo(point, 2);
                    }
                }
                drawArea.Refresh();
            }
            drawArea.Refresh();
        }

        public override void OnMouseUp(DrawArea drawArea, System.Windows.Forms.MouseEventArgs e)
        {
            //��¼ֱ���������ӵ�Ԫ������źͽ����
            DrawLine d = (drawArea.GraphicsList[0]) as DrawLine;
            CF.ChangeLineLink(drawArea, d, e);

            if (drawArea.IsTest == true)
            {
                //�������߲���
                CF.BeginTest(drawArea);
            }

            drawArea.GraphicsList[0].Normalize();
            drawArea.ActiveTool = DrawToolType.Pointer;
            drawArea.Capture = false;
            drawArea.Refresh();
        }
    }
}
