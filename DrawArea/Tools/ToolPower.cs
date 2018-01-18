using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;


namespace DrawArea
{
    public class ToolPower : ToolRectangle
    {
        public ToolPower()
        {
            byte[] obj = global::DrawArea.Properties.Resources.Power;
            Cursor = new Cursor(new System.IO.MemoryStream(obj));
        }
        public override void OnMouseDown(DrawArea drawArea, System.Windows.Forms.MouseEventArgs e)
        {
            DrawPower pow = new DrawPower(e.X, e.Y, 80, 53);
            //在结点链表里加结点
            VertexNode temp = new VertexNode(ComFunction.NewNodeNumber());
            pow.ObjectID = ComFunction.NewEquipmentNumber();
            temp.BelongsIndex = pow.ObjectID;

            base.AddNewObject(drawArea, pow);
            base.AddNewNode(drawArea, temp);
        }
    }
}
