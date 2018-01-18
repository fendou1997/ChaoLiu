using System;
using System.Collections.Generic;
using System.Text;

namespace DrawArea
{
    /// <summary>
    /// 指针的模式
    /// </summary>
    internal enum PointerMode
    {
        /// <summary>
        /// 初始模式
        /// </summary>
        None,
        /// <summary>
        /// 净模式  图形外部点击
        /// </summary>
        Net,
        /// <summary>
        /// 移动图元模式  图形内部点击
        /// </summary>
        Move,
        /// <summary>
        /// 改变图元尺寸模式 图形锚点上点击
        /// </summary>
        Size
    }
}
