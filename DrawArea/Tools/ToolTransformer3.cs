using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DrawArea
{
    public class ToolTransformer3 : ToolRectangle
    {
        private ComFunction CF = new ComFunction();
        public ToolTransformer3()
        {
            byte[] obj = global::DrawArea.Properties.Resources.Transformer3;
            Cursor = new Cursor(new System.IO.MemoryStream(obj));
        }
        public override void OnMouseDown(DrawArea drawArea, System.Windows.Forms.MouseEventArgs e)
        {
            DrawTransformer3 dt3 = new DrawTransformer3(e.X, e.Y, 80, 53);
            dt3.ObjectID = ComFunction.NewEquipmentNumber();
            base.AddNewObject(drawArea, dt3);
            //需要学生填写
            VertexNode temp1 = new VertexNode(ComFunction.NewNodeNumber());
            VertexNode temp2 = new VertexNode(ComFunction.NewNodeNumber());
            VertexNode temp3 = new VertexNode(ComFunction.NewNodeNumber());
            temp1.BelongsIndex = dt3.ObjectID;
            temp2.BelongsIndex = dt3.ObjectID;
            temp3.BelongsIndex = dt3.ObjectID;
            base.AddNewNode(drawArea, temp1);
            base.AddNewNode(drawArea, temp2);
            base.AddNewNode(drawArea, temp3);
            CF.AddEdge(drawArea, temp1, temp2);
            CF.AddEdge(drawArea, temp1, temp3);
            CF.AddEdge(drawArea, temp1, temp3);

        }
    }
}
