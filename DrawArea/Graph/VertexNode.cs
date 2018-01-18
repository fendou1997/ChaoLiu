using System;
using System.Collections.Generic;
using System.Text;

namespace DrawArea
{
    [Serializable]
    public class VertexNode
    {
        private int vertexName;// 获取或设置顶点的名字
        private bool visited;//获取或设置顶点是否被访问
        private EdgeNode firstContact;// 获取或设置顶点的第一个临接结点
        private int belongsIndex;// 该结点对应的设备在链表中的索引值

        public int VertexName
        {
            get
            {
                return vertexName;
            }
            set
            {
                vertexName = value;
            }
        }
        public bool Visited
        {
            get
            {
                return visited;
            }
            set
            {
                visited = value;
            }
        }
        public EdgeNode FirstContact
        {
            get
            {
                return firstContact;
            }
            set
            {
                firstContact = value;
            }
        }
        public int BelongsIndex
        {
            get
            {
                return belongsIndex;
            }
            set
            {
                belongsIndex = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="vName"></param>
        /// <param name="firstNode"></param>
        public VertexNode(int FirstName)
        {
            this.vertexName = FirstName;
            this.visited = false;
            this.firstContact = null;
        }
        public VertexNode(int FirstName, EdgeNode firstNode)
        {
            this.vertexName = FirstName;
            this.visited = false;
            this.firstContact = firstNode;
        }
    }
}
