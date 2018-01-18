using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using DrawArea;

namespace DrawArea
{
    [Serializable]
    public class GraphicsList
    {
        #region ���Բ���
        /// <summary>
        /// ��Ż��������豸
        /// </summary>
        private List<DrawObject> graphicsList;

        /// <summary>
        /// ��ȡ��ѡ��ͼ�ε��б�
        /// </summary>
        public List<DrawObject> Selection
        {
            get
            {
                List<DrawObject> lst = new List<DrawObject>();
                foreach (DrawObject o in graphicsList)
                {
                    if (o.Selected == true)
                    {
                        lst.Add(o);
                    }
                }
                return lst.Count == 0 ? null : lst;
            }
        }

        /// <summary>
        /// ��ȡͼԪ�����е�ͼԪ����
        /// </summary>
        public int Count
        {
            get
            {
                return graphicsList.Count;
            }
        }

        /// <summary>
        /// �õ���ѡ��ͼԪ�ĸ���
        /// </summary>
        public int SelectionCount
        {
            get
            {
                List<DrawObject> lst = Selection;

                return (lst == null) ? 0 : lst.Count;
            }
        }
        #endregion

        #region ���캯��
        public GraphicsList()
        {
            this.graphicsList = new List<DrawObject>();
        }
        #endregion

        #region �����ݽṹ�йصķ���
        /// <summary>
        /// ������������ͼ��
        /// </summary>
        /// <param name="obj"></param>
        public void Add(DrawObject obj)
        {
            graphicsList.Insert(0, obj);
        }

        /// <summary>
        /// �ڸ���λ�ò���ͼԪ
        /// </summary>
        /// <param name="index"></param>
        /// <param name="obj"></param>
        public void Insert(int index, DrawObject obj)
        {
            if (index >= 0 && index < graphicsList.Count)
            {
                graphicsList.Insert(index, obj);
            }
        }

        /// <summary>
        /// ɾ������λ�õ�ͼԪ
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            graphicsList.RemoveAt(index);
        }

        /// <summary>
        /// ɾ��ͼԪ
        /// </summary>
        public void Delete()
        {
            if (this.Count <= 0)
            {
                return;
            }
            for (int i = this.Count - 1; i >= 0; i--)
            {
                if (graphicsList[i].Selected == true)
                    this.RemoveAt(i);
            }
        }

        /// <summary>
        /// ɾ���������ͼԪ
        /// </summary>
        public void DeleteLastAddedObject()
        {
            if (graphicsList.Count > 0)
            {
                graphicsList.RemoveAt(0);
            }
        }

        /// <summary>
        /// �Ƴ������е�����ͼԪ
        /// </summary>
        /// <returns></returns>
        public void Clear()
        {
            if (graphicsList != null)
                graphicsList.Clear();
        }

        /// <summary>
        /// �ø�����ͼԪ�滻����λ�õ�ͼԪ
        /// </summary>
        /// <param name="index"></param>
        /// <param name="obj"></param>
        public void Replace(int index, DrawObject obj)
        {
            if (index >= 0 && index < graphicsList.Count)
            {
                graphicsList.RemoveAt(index);
                graphicsList.Insert(index, obj);
            }
        }
        #endregion

        #region �Ҽ��˵�������
        /// <summary>
        /// ѡ������ͼԪ
        /// </summary>
        public void SelectAll()
        {
            foreach (DrawObject o in graphicsList)
            {
                o.Selected = true;
            }
        }

        /// <summary>
        /// ����ͼ������Ϊδ��ѡ��״̬
        /// </summary>
        public void UnselectAll()
        {
            foreach (DrawObject obj in graphicsList)
            {
                obj.Selected = false;
            }
        }

        /// <summary>
        /// ѡ�񱻸������ο�ס��ͼԪ
        /// </summary>
        /// <param name="rectangle"></param>
        public void SelectInRectangle(Rectangle rectangle)
        {
            UnselectAll();

            foreach (DrawObject o in graphicsList)
            {
                if (o.IntersectsWith(rectangle))
                    o.Selected = true;
            }
        }

        /// <summary>
        /// �ƶ���ǰ��
        /// </summary>
        public void RemoveFirst()
        {
            if (this.SelectionCount <= 0)
                return;
            List<DrawObject> lst = new List<DrawObject>();
            for (int i = this.Count - 1; i >= 0; i--)
            {
                if (graphicsList[i].Selected == true)
                {
                    lst.Add(graphicsList[i]);
                    graphicsList.RemoveAt(i);
                }
            }
            lst.AddRange(graphicsList);
            graphicsList = lst;
        }

        /// <summary>
        /// �ƶ������
        /// </summary>
        public void RemoveLast()
        {
            if (this.SelectionCount <= 0)
                return;
            List<DrawObject> lst = new List<DrawObject>();
            for (int i = this.Count - 1; i >= 0; i--)
            {
                if (graphicsList[i].Selected == true)
                {
                    lst.Add(graphicsList[i]);
                    graphicsList.RemoveAt(i);
                }
            }
            graphicsList.AddRange(lst);
        }
        
        #endregion

        /// <summary>
        /// ͼ���б��������
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DrawObject this[int index]
        {
            get
            {
                if (index < 0 || index >= graphicsList.Count)
                    return null;

                return graphicsList[index];
            }
        }

        /// <summary>
        /// �������е�ͼ�ν��л���--������ĵײ�һֱ���������λ���
        /// ע�������ײ�����������ͼ
        /// </summary>
        /// <param name="g"></param>
        public void Draw(Graphics g)
        {
            for (int i = graphicsList.Count - 1; i >= 0; i--)
            {
                DrawObject obj = graphicsList[i];
                obj.Draw(g);

                if (obj.Selected == true)
                {
                    obj.DrawTracker(g);
                    obj.DrawName(g);
                }
            }
        }


    }
}
