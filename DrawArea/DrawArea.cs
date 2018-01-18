using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DrawArea
{
    public partial class DrawArea : UserControl
    {
        #region 属性部分
        private ToolObject[] tools;
        /// <summary>
        /// 获取或设置当前激活的绘图工具
        /// </summary>
        public DrawToolType ActiveTool;
        /// <summary>
        /// 获取或设置存储画图区域中图形的图形列表
        /// </summary>
        public GraphicsList GraphicsList;
        /// <summary>
        /// 获取或设置存储画图区域中图形对应的结点列表
        /// </summary>
        public AdGraph GraphicsNodeList;
        /// <summary>
        /// 用于判断绘图区此时是否处于测试过程
        /// </summary>
        public bool IsTest = false;
        //加
        public object[] o;
        #endregion

        #region 构造函数
        public DrawArea()
        {
            InitializeComponent();
        }
        #endregion

        #region 事件部分
        private void DrawArea_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.ActiveTool = DrawToolType.Pointer;
            this.GraphicsList = new GraphicsList();
            this.GraphicsNodeList = new AdGraph();
            o = new object[2];
            o[0] = this.GraphicsList;
            o[1] = this.GraphicsNodeList;

            tools = new ToolObject[(int)DrawToolType.NumberOfDrawEquipments];
            tools[(int)DrawToolType.Pointer] = new ToolPointer();
            tools[(int)DrawToolType.Transformers2] = new ToolTransformer2();
            tools[(int)DrawToolType.Transformers3] = new ToolTransformer3();
            tools[(int)DrawToolType.Knife] = new ToolKnife();
            tools[(int)DrawToolType.Capacitor] = new ToolCapacitor();
            tools[(int)DrawToolType.Line] = new ToolLine();
            tools[(int)DrawToolType.Power] = new ToolPower();
            tools[(int)DrawToolType.Break] = new ToolBreak();
        }

        private void DrawArea_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                tools[(int)this.ActiveTool].OnMouseDown(this, e);
            }
        }

        private void DrawArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.None)
            {
                tools[(int)this.ActiveTool].OnMouseMove(this, e);
            }
            else
                this.Cursor = Cursors.Default;
        }

        private void DrawArea_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                tools[(int)this.ActiveTool].OnMouseUp(this, e);
            }
            Refresh();
        }

        private void DrawArea_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255));
            e.Graphics.FillRectangle(brush, this.ClientRectangle);

            if (this.GraphicsList != null)
                this.GraphicsList.Draw(e.Graphics);
            brush.Dispose();
        }

        private void DrawArea_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true)
            {
                tools[(int)this.ActiveTool].IsCtrlOn = true;
            }
            if (e.Shift == true)
            {
                tools[(int)this.ActiveTool].IsShiftOn = true;
            }
        }
        private void DrawArea_KeyUp(object sender, KeyEventArgs e)
        {
            tools[(int)this.ActiveTool].IsCtrlOn = false;
            tools[(int)this.ActiveTool].IsShiftOn = false;
        }
        #endregion



    }
}