using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DrawArea
{
    public class ToolBreak : ToolRectangle
    {
        public ToolBreak()
        {
            byte[] obj = global::DrawArea.Properties.Resources.Break;
            Cursor = new Cursor(new System.IO.MemoryStream(obj));
        }

        public override void OnMouseDown(DrawArea drawArea, System.Windows.Forms.MouseEventArgs e)
        {
            DrawBreak  bre = new DrawBreak (e.X, e.Y, 80, 53);
            bre.ObjectID = ComFunction.NewEquipmentNumber();
            base.AddNewObject(drawArea, bre );
            //需要学生填写
            VertexNode temp1 = new VertexNode(ComFunction.NewNodeNumber());
            VertexNode temp2 = new VertexNode(ComFunction.NewNodeNumber());
            temp1.BelongsIndex = bre.ObjectID;
            temp2.BelongsIndex = bre.ObjectID;
            base.AddNewNode(drawArea, temp1);
            base.AddNewNode(drawArea, temp2);
        }
    }
}
