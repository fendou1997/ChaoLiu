using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DrawArea
{
    public class ToolKnife : ToolRectangle 
    {
        public ToolKnife()
        {
            byte[] obj = global::DrawArea.Properties.Resources.Knife;
            Cursor = new Cursor(new System.IO.MemoryStream(obj));
        }

        public override void OnMouseDown(DrawArea drawArea, System.Windows.Forms.MouseEventArgs e)
        {
            DrawKnife  knf = new DrawKnife (e.X, e.Y, 80, 53);
            knf.ObjectID = ComFunction.NewEquipmentNumber();
            base.AddNewObject(drawArea, knf );
            //需要学生填写
            VertexNode temp1 = new VertexNode(ComFunction.NewNodeNumber());
            VertexNode temp2 = new VertexNode(ComFunction.NewNodeNumber());
            temp1.BelongsIndex = knf.ObjectID;
            temp2.BelongsIndex = knf.ObjectID;
            base.AddNewNode(drawArea, temp1);
            base.AddNewNode(drawArea, temp2);
        }
    }
}
