using System;
using System.Collections.Generic;
using System.Text;

namespace DrawArea
{
    [Serializable]
    /// <summary>
    /// 表示绘图工具的枚举类型
    /// </summary>
    public enum DrawToolType
    {
        /// <summary>
        /// 箭头
        /// </summary>
        Pointer = 0,
        /// <summary>
        /// 2绕组卷变压器
        /// </summary>
        Transformers2 = 1,
        /// <summary>
        /// 3绕组卷变压器
        /// </summary>
        Transformers3 = 2,
        /// <summary>
        /// 电容
        /// </summary>
        Capacitor = 3,
        /// <summary>
        /// 刀闸
        /// </summary>
        Knife = 4,
        /// <summary>
        /// 母线
        /// </summary>
        Line = 5,
        /// <summary>
        /// 电源
        /// </summary>
        Power = 6,
        /// <summary>
        /// 开关
        /// </summary>
        Break = 7,
        /// <summary>
        /// 设备数量
        /// </summary>
        NumberOfDrawEquipments = 8
    }
}
