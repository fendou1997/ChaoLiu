using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DrawArea
{
    public abstract class ToolObject
    {
        #region ���Բ���
        /// <summary>
        /// ��ȡ�����ù��ߵĹ��
        /// </summary>
        protected Cursor Cursor;
        /// <summary>
        /// ��ȡ�������Ƿ�����Ctrl��
        /// </summary>
        public bool IsCtrlOn = default(bool);
        /// <summary>
        /// ��ȡ�������Ƿ�����Shift��
        /// </summary>
        public bool IsShiftOn = default(bool);
        #endregion

        #region ��������
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public abstract void OnMouseDown(DrawArea drawArea, MouseEventArgs e);
        /// <summary>
        /// �ƶ���꣨�����������»���û�а��κ�������
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public abstract void OnMouseMove(DrawArea drawArea, MouseEventArgs e);
        /// <summary>
        /// ���������ͷ�
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public abstract void OnMouseUp(DrawArea drawArea, MouseEventArgs e);
        /// <summary>
        /// �Ѹ�ͼ����ӵ���ͼ������
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
        /// �Ѹý����ӵ���ͼ������
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
