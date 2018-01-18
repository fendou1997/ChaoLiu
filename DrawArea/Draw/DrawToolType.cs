using System;
using System.Collections.Generic;
using System.Text;

namespace DrawArea
{
    [Serializable]
    /// <summary>
    /// ��ʾ��ͼ���ߵ�ö������
    /// </summary>
    public enum DrawToolType
    {
        /// <summary>
        /// ��ͷ
        /// </summary>
        Pointer = 0,
        /// <summary>
        /// 2������ѹ��
        /// </summary>
        Transformers2 = 1,
        /// <summary>
        /// 3������ѹ��
        /// </summary>
        Transformers3 = 2,
        /// <summary>
        /// ����
        /// </summary>
        Capacitor = 3,
        /// <summary>
        /// ��բ
        /// </summary>
        Knife = 4,
        /// <summary>
        /// ĸ��
        /// </summary>
        Line = 5,
        /// <summary>
        /// ��Դ
        /// </summary>
        Power = 6,
        /// <summary>
        /// ����
        /// </summary>
        Break = 7,
        /// <summary>
        /// �豸����
        /// </summary>
        NumberOfDrawEquipments = 8
    }
}
