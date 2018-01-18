using System;
using System.Collections.Generic;
using System.Text;

namespace DrawArea
{
    [Serializable]
    public class AdGraph
    {
        private List<VertexNode> vertexList;//结点表

        public List<VertexNode> VertexList
        {
            get
            {
                return vertexList;
            }
            set
            {
                vertexList = value;
            }
        }
        
        public int VertexCount//获取结点个数
        {
            get
            {
                return vertexList.Count;
            }
        }

        public AdGraph()
        {
            vertexList = new List<VertexNode>();
        }
        /// <summary>
        /// 添加结点
        /// </summary>
        /// <param name="snode"></param>
        public void Add(VertexNode snode)
        {
            vertexList.Add(snode);
        }

        /// <summary>
        /// 根据结点ID号找到相应的索引值
        /// </summary>
        /// <param name="ID">结点ID</param>
        /// <returns></returns>
        public int FindIndex(int ID)
        {
            int index = -1;
            for (int i = 0; i < vertexList.Count; i++)
            {
                if (vertexList[i].VertexName == ID)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
       /// <summary>
       /// 删除结点
       /// </summary>
       /// <param name="ID"></param>
        public void Remove(int ID)
        {
            vertexList.RemoveAt(FindIndex(ID));
        }
        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public VertexNode this[int index]
        {
            get
            {
                if (index < 0 || index > vertexList.Count - 1)
                    throw new Exception("索引位置无效");
                return vertexList[index] == null ? null : vertexList[index];
            }
            set
            {
                if (index < 0 || index > vertexList.Count - 1)
                    throw new Exception("索引位置无效");
                vertexList[index] = value;
            }
        }

        /// <summary>
        /// 根据编号得到首结点
        /// </summary>
        /// <param name="ID">首结点编号</param>
        /// <returns></returns>
        //public VertexNode FindNode(int ID)
        //{
        //    int i;
        //    VertexNode Result = new VertexNode(0);
        //    for (i = 0; i < vertexList.Count; i++)
        //    {
        //        if (vertexList[i].VertexName == ID)
        //        {
        //            Result = vertexList[i];
        //        }
        //    }
        //    return i == vertexList.Count ? Result : null;

        //}

        /// <summary>
        /// 加边函数
        /// </summary>
        /// <param name="Firstnode">首结点</param>
        /// <param name="Edgenode">边结点</param>
        public void AddEdge(int Firstnode, int Edgenode)
        {
            //得到索引值i,j
            int i = FindIndex(Firstnode);
            int j = FindIndex(Edgenode);
            EdgeNode temp = vertexList[i].FirstContact;
            if (temp == null)
            {
                vertexList[i].FirstContact = new EdgeNode(j);
            }
            else
            {
                while (temp.Next != null)
                {
                    temp = temp.Next;
                    //如果已经有这条边了则返回
                    if (temp.Index == j)
                        return;
                }
                temp.Next = new EdgeNode(j);
            }
        }

        /// <summary>
        /// 减边函数
        /// </summary>
        /// <param name="Firstnode"></param>
        /// <param name="Edgenode"></param>
        public void ReduceEdge(int Firstnode, int Edgenode)
        {
            int i = FindIndex(Firstnode);
            int j = FindIndex(Edgenode);
            EdgeNode temp = vertexList[i].FirstContact;
            if (temp != null)
            {
                if (temp.Index == j)
                {
                    //第一个边结点是要减去的边             
                    if (temp.Next == null)
                    {
                        //如果该结点后面没有边结点
                        vertexList[i].FirstContact = null;
                    }
                    else
                    {
                        //如果该结点后面还有边结点
                        vertexList[i].FirstContact = temp.Next;
                    }
                }
                else
                {
                    //第一个边结点不是要减去的边 
                    EdgeNode temp1;
                    while (temp.Next != null)
                    {
                        temp1 = temp.Next;
                        //有这条边再去减
                        if (temp1.Index == j)
                        {
                            temp.Next = temp1.Next;
                        }
                        else
                        {
                            temp = temp.Next;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 深度优先搜索递归函数
        /// </summary>
        /// <param name="i">索引值</param>
        private void DFS(int i)
        {
            vertexList[i].Visited = true;
            EdgeNode p = vertexList[i].FirstContact;
            while (p != null)
            {
                if (vertexList[FindIndex(p.Index)].Visited == true)
                    p = p.Next;
                else
                    DFS(FindIndex(p.Index));
            }
        }
        /// <summary>
        /// 深度优先搜索
        /// </summary>
        /// <param name="Firstnode"></param>
        /// <returns></returns>
        public void DFSTraversal(List<int> Firstnode)
        {

            //将所有结点访问属性设为false
            for (int j = 0; j < vertexList.Count; j++)
            {
                vertexList[j].Visited = false;
            }
            for (int k = 0; k < Firstnode.Count; k++)
            {
                int i = FindIndex(Firstnode[k]);
                if (i != -1)
                {
                    DFS(i);
                }
            }
        }
    }
}
