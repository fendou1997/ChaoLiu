using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DrawArea
{
    public class ComFunction
    {
        public static int EquipmentID = 1;
        public static int NodeID = 0;

        /// <summary>
        /// 用于对新设备的编号
        /// </summary>
        /// <returns>新设备编号</returns>
        public static int NewEquipmentNumber()
        {
            return EquipmentID++;
        }

        /// <summary>
        /// 对于新结点的编号
        /// </summary>
        /// <returns>新结点编号</returns>
        public static int NewNodeNumber()
        {
            return NodeID++;
        }

        /// <summary>
        /// //鼠标在结点范围内就对该结点进行编号
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public void NodeRange(DrawArea drawArea, System.Windows.Forms.MouseEventArgs e)
        {
            for (int i = 0; i < drawArea.GraphicsList.Count; i++)
            {
                int num = drawArea.GraphicsList[i].NodeCount;
                drawArea.GraphicsList[i].Node = -1;
                drawArea.Refresh();
                for (int j = 0; j < num; j++)
                {
                    //j用来记录该元器件的结点编号
                    Point Newpoint = drawArea.GraphicsList[i].GetNode(j + 1);
                    double distance = Math.Sqrt((Newpoint.X - e.X) * (Newpoint.X - e.X) + (Newpoint.Y - e.Y) * (Newpoint.Y - e.Y));
                    if (distance <= 10)
                    {
                        drawArea.GraphicsList[i].Node = j + 1;
                        drawArea.Refresh();
                    }
                }
            }
        }

        /// <summary>
        /// 找到被鼠标击中的结点的坐标
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public Point OnRange(DrawArea drawArea)
        {
            Point temp = new Point();
            bool judge = false;
            int i, j;
            for (i = 0; i < drawArea.GraphicsList.Count; i++)
            {
                j = drawArea.GraphicsList[i].Node;
                if (j > 0)
                {
                    temp = drawArea.GraphicsList[i].GetNode(j);
                    judge = true;
                    break;
                }
            }
            return judge ? temp : new Point(-1, -1);
        }

        #region 寻找函数

        /// <summary>
        /// 找出与元器件drawObject结点相连的直线锚点信息
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="resizedObject">输入一个要得出直线锚点的元器件</param>
        /// <returns>返回一个数组第一列表示该元器件的结点，第二列表示与该结点连接的直线索引，第三列表示直线的锚点编号</returns>
        public List<int[]> FindLineHandle(DrawArea drawArea, DrawObject drawObject)
        {
            List<int[]> temp = new List<int[]>();
            int i, j, k;
            Point Position;
            double distance = 100;
            for (i = 0; i < drawArea.GraphicsList.Count; i++)
            {
                DrawObject temp1 = drawArea.GraphicsList[i];
                if (temp1 is DrawLine)
                {
                    for (j = 1; j <= drawObject.NodeCount; j++)
                    {
                        //得到每个结点的坐标，寻找与该结点相连的直线
                        Position = drawObject.GetNode(j);

                        for (k = 1; k <= 2; k++)
                        {
                            Point p = temp1.GetHandle(k);
                            distance = Math.Sqrt((Position.X - p.X) * (Position.X - p.X) + (Position.Y - p.Y) * (Position.Y - p.Y));
                            if (distance < 10)
                            {
                                DrawLine DL = temp1 as DrawLine;
                                if (DL.FindEquipmentID[k - 1] == drawObject.ObjectID && DL.FindNodeID[k - 1] == j)
                                {
                                    //该直线k锚点就是连接的这个结点
                                    temp.Add(new int[] { j, i, k });
                                    break;
                                }
                                else if (DL.ConncetSNodeCount() == 0)
                                {
                                    //如果线的两端都没有连结点
                                    temp.Add(new int[] { j, i, k });
                                    DL.FindNodeID[k - 1] = j;
                                    DL.FindEquipmentID[k - 1] = drawObject.ObjectID;
                                    break;
                                }
                                else if (DL.ConncetSNodeCount() == 1 && DL.FindEquipmentID[k - 1] == 0)
                                {
                                    //如果线只连接了一个其他元器件的结点,则可以将该线没有连接结点的锚点于结点连接
                                    temp.Add(new int[] { j, i, k });
                                    DL.FindNodeID[k - 1] = j;
                                    DL.FindEquipmentID[k - 1] = drawObject.ObjectID;
                                    AddEdge(drawArea, DL);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return temp;
        }

        /// <summary>
        /// 通过设备ID和结点信息得到结点坐标
        /// </summary>
        /// <returns></returns>
        public Point FindPoint(DrawArea drawArea, int EquipmentID, int NodeID)
        {
            if (NodeID > 0)
            {
                int i;
                Point p = new Point(0, 0);
                for (i = 0; i < drawArea.GraphicsList.Count; i++)
                {
                    if (drawArea.GraphicsList[i].ObjectID == EquipmentID)
                    {
                        p = drawArea.GraphicsList[i].GetNode(NodeID);
                        break;
                    }
                }
                return i == drawArea.GraphicsList.Count ? new Point(-1, -1) : p;
            }
            else
            {
                return new Point(-1, -1);
            }
        }

        /// <summary>
        /// 通过设备ID号和结点ID寻找首结点链表中的ID号
        /// </summary>
        /// <returns></returns>
        public int FindVertextID(DrawArea drawArea, int EquipmentID, int NodeID)
        {
            if (EquipmentID != 0 && NodeID != 0)
            {
                List<int> temp = new List<int>(); //用于记录EquipmentID对应的设备的所有首结点ID
                for (int i = 0; i < drawArea.GraphicsNodeList.VertexCount; i++)
                {
                    if (drawArea.GraphicsNodeList.VertexList[i].BelongsIndex == EquipmentID)
                    {
                        //如果结点链表中与设备相连的ID与输入的ID号相同
                        temp.Add(drawArea.GraphicsNodeList.VertexList[i].VertexName);
                    }
                }
                return temp[NodeID - 1];
            }
            else
            {
                return -1;
            }
        }
        #endregion

        /// <summary>
        /// 改变连线状态
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="drawLine"></param>
        public void ChangeLineLink(DrawArea drawArea, DrawLine drawLine, System.Windows.Forms.MouseEventArgs e)
        {
            int i, j = -1, k;
            for (i = 0; i < drawArea.GraphicsList.Count; i++)
            {
                j = drawArea.GraphicsList[i].Node;
                if (j > 0)
                {
                    break;
                }
            }
            if (j != -1)
            {
                //在结点范围之内
                Point position = drawArea.GraphicsList[i].GetNode(j);
                k = drawLine.HitTest(position);
            }
            else
            {
                k = drawLine.HitTest(e.Location);
            }
            if (k == 1 || k == 2)
            {
                if (j != -1)
                {
                    //如果之前就有边则先减边
                    ReduceEdge(drawArea, drawLine);
                    //说明有击中一个结点
                    drawLine.FindEquipmentID[k - 1] = drawArea.GraphicsList[i].ObjectID;
                    drawLine.FindNodeID[k - 1] = j;

                    //给直线连接的首结点加边
                    AddEdge(drawArea, drawLine);
                }
                else
                {
                    //应在之前减边
                    ReduceEdge(drawArea, drawLine);
                    //说明此时母线锚点没有击中结点
                    drawLine.FindEquipmentID[k - 1] = 0;
                    drawLine.FindNodeID[k - 1] = 0;
                }
            }
            else
            {
                //如果是在母线内部
                for (i = 0; i < 2; i++)
                {
                    if (drawLine.FindNodeID[i] != 0)
                    {
                        Point p = FindPoint(drawArea, drawLine.FindEquipmentID[i], drawLine.FindNodeID[i]);
                        Point newp = drawLine.GetHandle(i + 1);
                        double distance = Math.Sqrt((p.X - newp.X) * (p.X - newp.X) + (p.Y - newp.Y) * (p.Y - newp.Y));
                        if (distance > 10)
                        {
                            //如果有一个锚点不连着结点了则都另一个锚点也不连着了
                            if (drawLine.ConncetSNodeCount() == 2)
                            {
                                //如果之前直线两边都连着结点则先剪边
                                ReduceEdge(drawArea, drawLine);
                            }
                            drawLine.FindNodeID = new int[2];
                            drawLine.FindEquipmentID = new int[2];
                            break;
                        }
                    }
                }
            }
        }

        #region 减边函数重载
        /// <summary>
        /// 通过直线给结点减边
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="drawLine"></param>
        public void ReduceEdge(DrawArea drawArea, DrawLine drawLine)
        {
            int Firstnode, Edgenode;
            int count = drawLine.ConncetSNodeCount();
            if (count == 2)
            {
                //直线两边都连着结点，可以剪边
                //直线锚点1对应的首结点ID
                Firstnode = FindVertextID(drawArea, drawLine.FindEquipmentID[0], drawLine.FindNodeID[0]);
                //直线锚点2对应的首结点ID
                Edgenode = FindVertextID(drawArea, drawLine.FindEquipmentID[1], drawLine.FindNodeID[1]);
                //减边函数
                drawArea.GraphicsNodeList.ReduceEdge(Firstnode, Edgenode);
                drawArea.GraphicsNodeList.ReduceEdge(Edgenode, Firstnode);
            }
            else
            {
                return;
            }
        }
        /// <summary>
        /// 通过开关状态给开关对应的首结点减边
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="drawBreak"></param>
        public void ReduceEdge(DrawArea drawArea, DrawBreak drawBreak)
        {
            //如果开关之前打开则可以减边
            int StartVertexID, EndVertexID;
            //得到首结点的ID
            StartVertexID = FindVertextID(drawArea, drawBreak.ObjectID, 1);
            EndVertexID = FindVertextID(drawArea, drawBreak.ObjectID, 2);
            //调用连线函数
            drawArea.GraphicsNodeList.ReduceEdge(StartVertexID, EndVertexID);
            drawArea.GraphicsNodeList.ReduceEdge(EndVertexID, StartVertexID);

        }
        /// <summary>
        /// 通过刀闸状态给刀闸对应的首结点减边
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="drawBreak"></param>
        public void ReduceEdge(DrawArea drawArea, DrawKnife drawKnife)
        {
            //如果刀闸之前闭合则可以减边
            int StartVertexID, EndVertexID;
            //得到首结点的ID
            StartVertexID = FindVertextID(drawArea, drawKnife.ObjectID, 1);
            EndVertexID = FindVertextID(drawArea, drawKnife.ObjectID, 2);
            //调用连线函数
            drawArea.GraphicsNodeList.ReduceEdge(StartVertexID, EndVertexID);
            drawArea.GraphicsNodeList.ReduceEdge(EndVertexID, StartVertexID);

        }
        /// <summary>
        /// 通过刀闸状态给刀闸对应的首结点减边
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="drawBreak"></param>
        public void ReduceEdge(DrawArea drawArea, int StartVertexID, int EndVertexID)
        {
            drawArea.GraphicsNodeList.ReduceEdge(StartVertexID, EndVertexID);
            drawArea.GraphicsNodeList.ReduceEdge(EndVertexID, StartVertexID);
        }
        #endregion

        #region 加边函数重载
        /// <summary>
        /// 通过直线给首结点加边
        /// </summary>
        /// <param name="drawArea">画图区域</param>
        /// <param name="drawLine">直线</param>
        public void AddEdge(DrawArea drawArea, DrawLine drawLine)
        {
            int StartVertexID, EndVertexID;
            if (drawLine.ConncetSNodeCount() == 2)
            {
                //直线两边都连着结点，可以剪边
                //直线锚点1对应的首结点ID
                StartVertexID = FindVertextID(drawArea, drawLine.FindEquipmentID[0], drawLine.FindNodeID[0]);
                //直线锚点2对应的首结点ID
                EndVertexID = FindVertextID(drawArea, drawLine.FindEquipmentID[1], drawLine.FindNodeID[1]);
                //调用加边函数
                drawArea.GraphicsNodeList.AddEdge(StartVertexID, EndVertexID);
                drawArea.GraphicsNodeList.AddEdge(EndVertexID, StartVertexID);

            }
            else
            {
                return;
            }
        }
        /// <summary>
        /// 通过结点信息给首结点加边
        /// </summary>
        /// <param name="drawArea">画图区域</param>
        /// <param name="VerNode1">首结点</param>
        /// <param name="VerNode2">另一个首结点</param>
        public void AddEdge(DrawArea drawArea, VertexNode VerNode1, VertexNode VerNode2)
        {
            //调用加边函数
            drawArea.GraphicsNodeList.AddEdge(VerNode1.VertexName, VerNode2.VertexName);
            drawArea.GraphicsNodeList.AddEdge(VerNode2.VertexName, VerNode1.VertexName);
        }

        /// <summary>
        /// 通过开关状态给开关对应的首结点加边
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="drawBreak"></param>
        public void AddEdge(DrawArea drawArea, DrawBreak drawBreak)
        {

            //开关之前关闭则可以调用该方法
            int StartVertexID, EndVertexID;
            //得到首结点的ID
            StartVertexID = FindVertextID(drawArea, drawBreak.ObjectID, 1);
            EndVertexID = FindVertextID(drawArea, drawBreak.ObjectID, 2);
            //调用连线函数
            drawArea.GraphicsNodeList.AddEdge(StartVertexID, EndVertexID);
            drawArea.GraphicsNodeList.AddEdge(EndVertexID, StartVertexID);

        }
        /// <summary>
        /// 通过刀闸状态给刀闸对应的首结点加边
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="drawBreak"></param>
        public void AddEdge(DrawArea drawArea, DrawKnife drawKnife)
        {

            //刀闸之前打开则可以调用该方法
            int StartVertexID, EndVertexID;
            //得到首结点的ID
            StartVertexID = FindVertextID(drawArea, drawKnife.ObjectID, 1);
            EndVertexID = FindVertextID(drawArea, drawKnife.ObjectID, 2);
            //调用连线函数
            drawArea.GraphicsNodeList.AddEdge(StartVertexID, EndVertexID);
            drawArea.GraphicsNodeList.AddEdge(EndVertexID, StartVertexID);

        }
        #endregion


        /// <summary>
        /// 进行深度优先搜索（）
        /// </summary>
        /// <param name="drawArea"></param>
        public void Do_DFS(DrawArea drawArea)
        {
            List<int> temp = new List<int>();  //用于记录电源的编号
            for (int i = 0; i < drawArea.GraphicsList.Count; i++)
            {
                if (drawArea.GraphicsList[i] is DrawPower)
                {
                    DrawPower temp1 = drawArea.GraphicsList[i] as DrawPower;
                    temp.Add(FindVertextID(drawArea, temp1.ObjectID, 1));
                }
            }
            drawArea.GraphicsNodeList.DFSTraversal(temp);

        }
        /// <summary>
        /// 开始测试
        /// </summary>
        /// <param name="drawAreaClient"></param>
        public void BeginTest(DrawArea drawAreaClient)
        {
            //将所有的元器件带电状态设为false
            for (int i = 0; i < drawAreaClient.GraphicsList.Count; i++)
            {
                drawAreaClient.GraphicsList[i].IsPowerOn = false;
            }

            //先进行深度优先搜索
            Do_DFS(drawAreaClient);
            for (int j = 0; j < drawAreaClient.GraphicsList.Count; j++)
            {
                bool judge = false;
                if (drawAreaClient.GraphicsList[j] is DrawLine)
                {
                    //如果元器件是母线，母线任意一端的元器件结点带电则该母线带点
                    DrawLine drawLine = drawAreaClient.GraphicsList[j] as DrawLine;
                    for (int k = 0; k < 2; k++)
                    {
                        int ID = FindVertextID(drawAreaClient, drawLine.FindEquipmentID[k], drawLine.FindNodeID[k]);
                        if (ID != -1)
                        {
                            int index = drawAreaClient.GraphicsNodeList.FindIndex(ID);
                            VertexNode VNode = drawAreaClient.GraphicsNodeList[index];
                            if (VNode.Visited == true)
                            {
                                judge = true;
                                break;
                            }
                        }
                    }
                    drawLine.IsPowerOn = judge;
                }
                else if (drawAreaClient.GraphicsList[j] is DrawBreak)
                {
                    //如果是开关则需要看开关是否打开
                    DrawBreak DB = drawAreaClient.GraphicsList[j] as DrawBreak;

                    //如果开关打开
                    if (DB.OpenOrClose)
                    {
                        int count = 0;
                        //如果说某元器件里所有的首结点都带电则该元器件带电
                        for (int k = 1; k <= drawAreaClient.GraphicsList[j].NodeCount; k++)
                        {
                            int ID = FindVertextID(drawAreaClient, DB.ObjectID, k);
                            int index = drawAreaClient.GraphicsNodeList.FindIndex(ID);
                            VertexNode VNode = drawAreaClient.GraphicsNodeList[index];
                            if (VNode.Visited == true)
                            {
                                count += 1;
                            }
                        }
                        if (count == DB.NodeCount)
                        {
                            judge = true;
                        }
                        DB.IsPowerOn = judge;
                    }
                    else
                    {
                        DB.IsPowerOn = false;
                    }
                }
                else if (drawAreaClient.GraphicsList[j] is DrawKnife)
                {
                    //如果是刀闸则需要看开关是否打开
                    DrawKnife DK = drawAreaClient.GraphicsList[j] as DrawKnife;

                    //如果刀闸打开
                    if (DK.OpenOrClose)
                    {
                        int count = 0;
                        //如果说某元器件里所有的首结点都带电则该元器件带电
                        for (int k = 1; k <= drawAreaClient.GraphicsList[j].NodeCount; k++)
                        {
                            int ID = FindVertextID(drawAreaClient, DK.ObjectID, k);
                            int index = drawAreaClient.GraphicsNodeList.FindIndex(ID);
                            VertexNode VNode = drawAreaClient.GraphicsNodeList[index];
                            if (VNode.Visited == true)
                            {
                                count += 1;
                            }
                        }
                        if (count == DK.NodeCount)
                        {
                            judge = true;
                        }
                        DK.IsPowerOn = judge;
                    }
                    else
                    {
                        DK.IsPowerOn = false;
                    }
                }
                else
                {
                    int count = 0;
                    DrawObject DO = drawAreaClient.GraphicsList[j];
                    //如果说某元器件里所有的首结点都带电则该元器件带电
                    for (int k = 1; k <= drawAreaClient.GraphicsList[j].NodeCount; k++)
                    {
                        int ID = FindVertextID(drawAreaClient, DO.ObjectID, k);
                        int index = drawAreaClient.GraphicsNodeList.FindIndex(ID);
                        VertexNode VNode = drawAreaClient.GraphicsNodeList[index];
                        if (VNode.Visited == true)
                        {
                            count += 1;
                        }
                    }
                    if (count == DO.NodeCount)
                    {
                        judge = true;
                    }
                    DO.IsPowerOn = judge;
                }
            }
            drawAreaClient.Refresh();
        }

        public void StopTest(DrawArea drawAreaClient)
        {
            for (int i = 0; i < drawAreaClient.GraphicsList.Count; i++)
            {
                drawAreaClient.GraphicsList[i].IsPowerOn = false;
            }
        }

        public void BreakOperate(DrawArea drawArea, DrawBreak drawBreak)
        {
            if (drawBreak.OpenOrClose == true)
            {

                AddEdge(drawArea, drawBreak);//给开关两端的首结点连线
                Do_DFS(drawArea);
                BeginTest(drawArea);
            }
            else
            {

                ReduceEdge(drawArea, drawBreak);//断开开关两端的首结点连线
                Do_DFS(drawArea);
                BeginTest(drawArea);
            }
        }

        public void KnifeOperate(DrawArea drawArea, DrawKnife drawKnife)
        {
            if (drawKnife.OpenOrClose == true)
            {
                AddEdge(drawArea, drawKnife);
                Do_DFS(drawArea);
                BeginTest(drawArea);
            }
            else
            {
                ReduceEdge(drawArea, drawKnife);
                Do_DFS(drawArea);
                BeginTest(drawArea);
            }
        }
    }
}
