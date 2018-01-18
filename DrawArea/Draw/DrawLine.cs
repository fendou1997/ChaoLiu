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
        #region ���Բ���
        [NonSerialized]
        protected GraphicsPath areaPath;
        [NonSerialized]
        protected Pen areaPen;
        [NonSerialized]
        protected Region areaRegion;
        /// <summary>
        /// ���
        /// </summary>
        public Point startPoint;
        /// <summary>
        /// �յ�
        /// </summary>
        public Point endPoint;
        /// <summary>
        /// ��ȡê�����
        /// </summary>
        public override int HandleCount
        {
            get
            {
                return 2;
            }
        }
        public int[] FindEquipmentID = new int[2];//ֱ��ê���ӦԪ��������
        public int[] FindNodeID = new int[2];//�����ӵĽ���
        #endregion

        #region ���캯��
        public DrawLine(int x1, int y1, int x2, int y2)
            : base()
        {
            base.Name = "ĸ��";
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
            base.Name = "ĸ��";
        }
        #endregion

        #region ��ͼ�β����йصķ���
        /// <summary>
        /// �ͷŻ�����
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
        /// ����ĸ��
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

        #region ��ê���йصķ���
        /// <summary>
        /// ���ݱ�ŵõ�ê������
        /// </summary>
        /// <param name="handleNumber">����ֱ��:���1,�յ�2.</param>
        /// <returns>ͼ��ê������</returns>
        public override Point GetHandle(int handleNumber)
        {
            if (handleNumber == 1)
                return startPoint;
            else
                return endPoint;
        }
        /// <summary>
        /// ���ݱ�ŵõ�ê�㴦�Ĺ��
        /// </summary>
        /// <param name="handleNumber">����ֱ��:���1,�յ�2.</param>
        /// <returns>ê�㴦���</returns>
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
        /// ����ê���źʹ������ĵ�����������λ��
        /// </summary>
        /// <param name="point"></param>
        /// <param name="handleNumber">����:���Ͻ�Ϊ1,���Ͻ�Ϊ2,���½�Ϊ3,���½�Ϊ4</param>
        public override void MoveHandleTo(Point point, int handleNumber)
        {
            if (handleNumber == 1)//����ƶ�
                startPoint = point;
            else if (handleNumber == 2)//�յ��ƶ�
                endPoint = point;
            else if (handleNumber == 10)//�±���ˮƽ�ƶ�
            {
                startPoint.X = point.X;
                startPoint.Y = endPoint.Y;
            }
            else if (handleNumber == 20)//�ϱ���ˮƽ�ƶ�
            {
                endPoint.X = point.X;
                endPoint.Y = startPoint.Y;
            }
            else if (handleNumber == 60)//����ߴ�ֱ�ƶ�
            {
                endPoint.X = startPoint.X;
                endPoint.Y = point.Y;
            }
            else//�ұ��ߴ�ֱ�ƶ�
            {
                startPoint.X = endPoint.X;
                startPoint.Y = point.Y;
            }
            this.Invalidate();
        }
        #endregion

        #region �ж�����λ���йصķ���
        /// <summary>
        /// �Ը����ĵ���в���
        /// ����+1 �õ����Ͻ�ê����
        /// ����+2 �õ����Ͻ�ê����
        /// ����0  �õ��ھ����ڲ�
        /// ����-1 �õ�û�������ڲ�Ҳû��ê�������ڲ�
        /// </summary>
        /// <param name="point">���в��Ե������</param>
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
        /// ����ֱ�ߵĻ����
        /// </summary>
        protected virtual void CreateObjects()
        {
            if (this.areaPath != null)
                return;
            //�������ж���ֱ�ߵ�·�����Ա�������ѡ��
            this.areaPath = new GraphicsPath();
            this.areaPen = new Pen(Color.Black, 7);
            this.areaPath.AddLine(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
            this.areaPath.Widen(areaPen);
            // ����ֱ�ߵĻ����
            this.areaRegion = new Region(areaPath);
        }
        /// <summary>
        /// �жϸ������Ƿ���ֱ�ߵ�������
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        protected override bool PointInObject(Point point)
        {
            this.CreateObjects();
            return areaRegion.IsVisible(point);
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
            return new Point(0, 0);   //�����øú���
        }

        public int ConncetSNodeCount()//�������ӵĽ����
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
