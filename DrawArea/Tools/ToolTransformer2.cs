using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DrawArea
{
    public class ToolTransformer2 : ToolRectangle
    {
        private ComFunction CF = new ComFunction();
        public ToolTransformer2()
        {
            byte[] obj = global::DrawArea.Properties.Resources.Transformer2;
            Cursor = new Cursor(new System.IO.MemoryStream(obj));
        }

        public override void OnMouseDown(DrawArea drawArea, System.Windows.Forms.MouseEventArgs e)
        {
            DrawTransformer2 dt2 = new DrawTransformer2(e.X, e.Y, 80, 53);
            dt2.ObjectID = ComFunction.NewEquipmentNumber ();
            base.AddNewObject(drawArea, dt2);

            VertexNode temp1 = new VertexNode(ComFunction.NewNodeNumber());
            VertexNode temp2 = new VertexNode(ComFunction.NewNodeNumber());
            temp1.BelongsIndex = dt2.ObjectID;
            temp2.BelongsIndex = dt2.ObjectID;
            base.AddNewNode(drawArea, temp1);
            base.AddNewNode(drawArea, temp2);
            CF.AddEdge(drawArea, temp1, temp2);
        }
    }
}
