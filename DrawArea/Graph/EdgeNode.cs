using System;
using System.Collections.Generic;
using System.Text;

namespace DrawArea
{
    [Serializable]
    public class EdgeNode
    {
        private int index;//结点在链表中的索引
        private EdgeNode next;//下一个临接结点
        
        public int Index
        {
            get
            {
                return index;
            }
        }
        public EdgeNode Next
        {
            get
            {
                return next;
            }
            set
            {
                next = value;
            }
        }

        public EdgeNode(int index)
        {
            if (index < 0)
                throw new Exception("索引位置无效");
            this.index = index;
            this.next = null;
        }
        public EdgeNode(int index, EdgeNode next)
        {
            if (index < 0)
                throw new Exception("索引位置无效");
            this.index = index;
            this.next = next;   
        }
    }
}
