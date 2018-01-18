using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DrawArea
{
    public class ToolCapacitor : ToolRectangle
    {
        private ComFunction CF = new ComFunction();
        public ToolCapacitor()
        {
            byte[] obj = global::DrawArea.Properties.Resources.Capacitor;
            Cursor = new Cursor(new System.IO.MemoryStream(obj));
        }

        public override void OnMouseDown(DrawArea drawArea, System.Windows.Forms.MouseEventArgs e)
        {
            DrawCapacitor  cap = new DrawCapacitor (e.X, e.Y, 80, 53);
            cap.ObjectID = ComFunction.NewEquipmentNumber();
            base.AddNewObject(drawArea, cap );
            //需要学生填写
            VertexNode temp1 = new VertexNode(ComFunction.NewNodeNumber());
            VertexNode temp2 = new VertexNode(ComFunction.NewNodeNumber());
            temp1.BelongsIndex = cap.ObjectID;
            temp2.BelongsIndex = cap.ObjectID;
            base.AddNewNode(drawArea, temp1);
            base.AddNewNode(drawArea, temp2);
            CF.AddEdge(drawArea, temp1, temp2);
        }
    }
}
