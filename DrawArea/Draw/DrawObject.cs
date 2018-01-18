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
        #region ���Բ���
        /// <summary>
        /// ��ȡ���������һ�λ�ͼ����ɫ
        /// </summary>
        public static Color LastUsedColor = Color.Black;
        /// <summary>
        /// ��ȡ���������һ��ʹ�û��ʵĴ�ϸ
        /// </summary>
        public static int LastUsedPenWidth = 1;

        /// <summary>
        /// ��ȡ������Ψһ��ʶ�豸�ı��
        /// </summary>
        public int ObjectID = default(int);
        /// <summary>
        /// ��ȡ�������豸����
        /// string.Empty��ʾstring�����ı�
        /// </summary>
        public string Name = string.Empty;
        /// <summary>
        /// ��ȡ�����ñ�ʾ�豸�Ƿ����
        /// </summary>
        public bool IsPowerOn = default(bool);
        /// <summary>
        /// ��ȡ������ͼ�ε���ɫ
        /// </summary>
        public Color Color;
        /// <summary>
        /// ��ȡ�����û��ʵĴ�ϸ
        /// ʹ��default(int)������get,set
        /// </summary>
        public int PenWidth = default(int);
        /// <summary>
        /// ��ȡ������ͼ���Ƿ�ѡ��ı�ʶ
        /// </summary>
        public bool Selected = default(bool);
        /// <summary>
        /// ��ȡ�������豸��ת�ĽǶ�
        /// </summary>
        public int Angle = 0;
        /// <summary>
        /// ��ȡ��������괥���Ľ��ţ�û�д���ʱΪ-1
        /// </summary>
        public int Node = -1;
        /// <summary>
        /// ��־�豸�Ŀ���״̬
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
        /// ��ȡê�����
        /// </summary>
        public abstract int HandleCount
        {
            get;
        }
        /// <summary>
        /// ��ȡ������
        /// </summary>
        public abstract int NodeCount
        {
            get;
        }
        #endregion

        #region ���캯��
        /// <summary>
        /// ���캯��
        /// </summary>
        public DrawObject()
        {
            this.Color = DrawObject.LastUsedColor;
            this.PenWidth = DrawObject.LastUsedPenWidth;
        }
        #endregion

        #region ��ͼ�β����йصķ���
        
        /// <summary>
        /// ����ͼ��
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
        /// �ƶ�ͼ��
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        public abstract void Move(int deltaX, int deltaY);
        
        /// <summary>
        /// �淶��ͼ��
        /// </summary>
        public virtual void Normalize()
        {
            ;
        }
        #endregion

        #region ��ê���йصķ���
        /// <summary>
        /// ���ݱ�ŵõ�ê������
        /// </summary>
        /// <param name="handleNumber">���ھ���:���Ͻ�Ϊ1,���Ͻ�Ϊ2,���½�Ϊ3,���½�Ϊ4;����ֱ��:���1,�յ�2.</param>
        /// <returns>ͼ��ê������</returns>
        public abstract Point GetHandle(int handleNumber);
        /// <summary>
        /// ����ê��ŵõ�����ê��ľ���
        /// </summary>
        /// <param name="handleNumber">���Ͻ�Ϊ1,���Ͻ�Ϊ2,���½�Ϊ3,���½�Ϊ4</param>
        /// <returns>ͼ��ê�����</returns>
        public virtual Rectangle GetHandleRectangle(int handleNumber)
        {
            Point point = GetHandle(handleNumber);
            return new Rectangle(point.X - 3, point.Y - 3, 7, 7);
        }
        /// <summary>
        /// ���ݱ�ŵõ�ê�㴦�Ĺ��
        /// </summary>
        /// <param name="handleNumber">���Ͻ�Ϊ1,���Ͻ�Ϊ2,���½�Ϊ3,���½�Ϊ4</param>
        /// <returns>ê�㴦���</returns>
        public abstract Cursor GetHandleCursor(int handleNumber);
        /// <summary>
        /// ����ê���źʹ������ĵ�����������λ��
        /// </summary>
        /// <param name="point"></param>
        /// <param name="handleNumber">����:���Ͻ�Ϊ1,���Ͻ�Ϊ2,���½�Ϊ3,���½�Ϊ4</param>
        public abstract void MoveHandleTo(Point point, int handleNumber);
        /// <summary>
        /// Ϊͼ�λ���ê��
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

        #region �ж�����λ���йصķ���
        /// <summary>
        /// �Ը����ĵ���в���
        /// </summary>
        /// <param name="point">���в��Ե������</param>
        /// <returns></returns>
        public abstract int HitTest(Point point);
        /// <summary>
        /// �жϸ������Ƿ���ͼ����
        /// </summary>
        /// <param name="point">���в��Ե������</param>
        /// <returns></returns>
        protected abstract bool PointInObject(Point point);
        #endregion

        #region �ж������λ���йصķ���
        /// <summary>
        /// �ж�ͼԪ�����������Ƿ���ڽ���
        /// </summary>
        /// <param name="rectangle">���Եľ���</param>
        /// <returns></returns>
        public abstract bool IntersectsWith(Rectangle rectangle);
        #endregion

        #region �����йصķ���
        /// <summary>
        /// ���ݱ�ŵõ��������
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public abstract Point GetNode(int NodeNumber);
        /// <summary>
        /// �����豸����
        /// </summary>
        /// <param name="g"></param>
        public virtual void DrawName(Graphics g)
        {
        }
        
        #endregion

    }
}
