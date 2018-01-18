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
        #region 属性部分
        /// <summary>
        /// 存放画出来的设备
        /// </summary>
        private List<DrawObject> graphicsList;

        /// <summary>
        /// 获取被选择图形的列表
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
        /// 获取图元链表中的图元个数
        /// </summary>
        public int Count
        {
            get
            {
                return graphicsList.Count;
            }
        }

        /// <summary>
        /// 得到被选择图元的个数
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

        #region 构造函数
        public GraphicsList()
        {
            this.graphicsList = new List<DrawObject>();
        }
        #endregion

        #region 与数据结构有关的方法
        /// <summary>
        /// 在链表顶部插入图形
        /// </summary>
        /// <param name="obj"></param>
        public void Add(DrawObject obj)
        {
            graphicsList.Insert(0, obj);
        }

        /// <summary>
        /// 在给定位置插入图元
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
        /// 删除给定位置的图元
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            graphicsList.RemoveAt(index);
        }

        /// <summary>
        /// 删除图元
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
        /// 删除最后插入的图元
        /// </summary>
        public void DeleteLastAddedObject()
        {
            if (graphicsList.Count > 0)
            {
                graphicsList.RemoveAt(0);
            }
        }

        /// <summary>
        /// 移除链表中的所有图元
        /// </summary>
        /// <returns></returns>
        public void Clear()
        {
            if (graphicsList != null)
                graphicsList.Clear();
        }

        /// <summary>
        /// 用给定的图元替换给定位置的图元
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

        #region 右键菜单的设置
        /// <summary>
        /// 选择所有图元
        /// </summary>
        public void SelectAll()
        {
            foreach (DrawObject o in graphicsList)
            {
                o.Selected = true;
            }
        }

        /// <summary>
        /// 所有图形设置为未被选择状态
        /// </summary>
        public void UnselectAll()
        {
            foreach (DrawObject obj in graphicsList)
            {
                obj.Selected = false;
            }
        }

        /// <summary>
        /// 选择被给定矩形框住的图元
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
        /// 移动到前端
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
        /// 移动到后端
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
        /// 图形列表的索引器
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
        /// 把链表中的图形进行绘制--从链表的底部一直到顶部依次绘制
        /// 注意从链表底部到顶部来画图
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
