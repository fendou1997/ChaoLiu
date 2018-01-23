using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;
using CCWin.Win32;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;
using CalculateEngine;

namespace PowerMonitor
{
   

    public partial class Form2 : Skin_DevExpress
    {
        public Form2()
        {
            InitializeComponent();

        }
        static int JieDian;//获得节点数
        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_Click(object sender, EventArgs e)
        {
            string strFilePath = OpenFile1();
        }
        /// <summary>
        /// 调用打开函数
        /// </summary>
        /// <returns></returns>
        public string OpenFile1()
    {
        string Name = "";
        OpenFileDialog Open1 = new OpenFileDialog();
        Open1.Title = "打开文件";
        Open1.InitialDirectory = Application.ExecutablePath;//显示初始目录
        Open1.RestoreDirectory = true;
        if (Open1.ShowDialog() == DialogResult.OK)
        {
            Name = Open1.FileName;//返回文件名加路径
            return Name;
        }
        else
        {
            return "";
        }

    }
        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 另存为事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strSave = Savas();
        }
        /// <summary>
        /// 保存函数
        /// </summary>
        /// <returns></returns>
        public string Savas()
        {
            string Name = "";
            SaveFileDialog Saveas = new SaveFileDialog();
            Saveas.Title = "另存为";
            Saveas.InitialDirectory = Application.ExecutablePath;
            Saveas.RestoreDirectory = true;
            Saveas.Filter = ".txt|*.txt";//设置文件的类型
            if (Saveas.ShowDialog() == DialogResult.OK)
            {
                Name = Saveas.FileName;
                return Name;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
       /// 弹出Form1得到节点数
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void Paint_Click(object sender, EventArgs e)
        {
            Form1 frm1 = new Form1(GetJieDian);
            frm1.Show();
        }
        void GetJieDian(string str)
        {           
            JieDian = Convert.ToInt16(str);
            DTCreate(JieDian);
            

        }

        List<CCWin.SkinControl.SkinTextBox> textboxlist = new List<CCWin.SkinControl.SkinTextBox> { };
        List<CCWin.SkinControl.SkinTextBox> textboxlist1 = new List<CCWin.SkinControl.SkinTextBox> { };
        List<CCWin.SkinControl.SkinComboBox> skincomboList = new List<CCWin.SkinControl.SkinComboBox> { };
        List<CCWin.SkinControl.SkinLabel> skinlabellist = new List<CCWin.SkinControl.SkinLabel> { };//连接节点
        List<CCWin.SkinControl.SkinLabel> skinlabellist1 = new List<CCWin.SkinControl.SkinLabel> { };//
        List<CCWin.SkinControl.SkinLabel> skinlabellist2 = new List<CCWin.SkinControl.SkinLabel> { };//

        List<CCWin.SkinControl.SkinLabel> panellist1 = new List<CCWin.SkinControl.SkinLabel> { };//
        List<CCWin.SkinControl.SkinLabel> panellist3 = new List<CCWin.SkinControl.SkinLabel> { };//创建泛型集合将数据进行存储

        /// <summary>
        /// 用于动态创建控件
        /// </summary>
        /// <param name="JieDian"></param>
        public void DTCreate(int JieDian)
        {
            int i;
            for ( i = 0; i < JieDian; i++)
            {

                CCWin.SkinControl.SkinLabel panellabel1 = new CCWin.SkinControl.SkinLabel();
                panellabel1.Name = "panellabel1" + i;
                panellabel1.Parent = this;
                panellabel1.Location = new System.Drawing.Point(5, 55 + i * 90);
                panellabel1.Size = new System.Drawing.Size(69, 17);
                panellabel1.Text = "节点" + "   "+" U： ";
                this.panel3.Controls.Add(panellabel1);
                panellist3.Add(panellabel1);
                //CCWin.SkinControl.SkinLabel panellabel2 = new CCWin.SkinControl.SkinLabel();
                //panellabel2.Name = "panellabel2" + i;
                //panellabel2.Parent = this;
                //panellabel2.Location = new System.Drawing.Point(169, 55 + i * 90);
                //panellabel2.Size = new System.Drawing.Size(17, 17);
                //panellabel2.Text = "∠";
                //this.panel3.Controls.Add(panellabel2);

                CCWin.SkinControl.SkinLabel panellabel3 = new CCWin.SkinControl.SkinLabel();
                panellabel3.Name = "panellabel3" + i;
                panellabel3.Parent = this;
                panellabel3.Location = new System.Drawing.Point(80, 55 + i * 90);
                panellabel3.Size = new System.Drawing.Size(200, 17);
                panellabel3.Text = "";
                this.panel3.Controls.Add(panellabel3);
                panellist1.Add(panellabel3);

                //CCWin.SkinControl.SkinLabel panellabel4 = new CCWin.SkinControl.SkinLabel();
                //panellabel4.Name = "panellabel4" + i;
                //panellabel4.Parent = this;
                //panellabel4.Location = new System.Drawing.Point(200, 55 + i * 90);
                //panellabel4.Size = new System.Drawing.Size(200, 17);
                //panellabel4.Text = "1";
                //panellabel4.Visible = true;
                //this.panel3.Controls.Add(panellabel4);
                //panellist2.Add(panellabel4);

                //创建textBox
                CCWin.SkinControl.SkinTextBox textbox = new CCWin.SkinControl.SkinTextBox();
                textbox.Name = "textBoxZD" + i;
                textbox.Parent = this;
                textbox.Location = new System.Drawing.Point(354, 27 + i * 90);
                textbox.Size = new System.Drawing.Size(81, 28);
                textbox.Text = "";
                textbox.Visible = false;
                this.panel1.Controls.Add(textbox);
                textboxlist.Add(textbox);

                CCWin.SkinControl.SkinTextBox textbox1 = new CCWin.SkinControl.SkinTextBox();
                textbox1.Name = "textBoxZD1" + i;
                textbox1.Parent = this;
                textbox1.Location = new System.Drawing.Point(512, 27 + i * 90);
                textbox1.Size = new System.Drawing.Size(81, 28);
                textbox1.Text = "";
                textbox1.Visible = false;
                this.panel1.Controls.Add(textbox1);
                textboxlist1.Add(textbox1);


                CCWin.SkinControl.SkinComboBox skincombo = new CCWin.SkinControl.SkinComboBox();
                skincombo.Name = "comboBox" + i;
                skincombo.Parent = this;
                skincombo.Location = new System.Drawing.Point(141, 27 + i * 90);
                skincombo.Size = new System.Drawing.Size(121, 22);
                skincombo.Items.Add("PQ节点");
                skincombo.Items.Add("PV节点");
                skincombo.Items.Add("平衡节点");
                this.panel1.Controls.Add(skincombo);
                skincomboList.Add(skincombo);
                //skincombo.SelectedIndexChanged += new System.EventHandler(skinCombo_SelectedIndexChanged);


                CCWin.SkinControl.SkinLabel label1 = new CCWin.SkinControl.SkinLabel();
                label1.Name = "labelZD1" + i;
                label1.Parent = this;
                label1.Location = new System.Drawing.Point(51, 33 + i * 90);
                label1.Size = new System.Drawing.Size(69, 17);
                label1.Text = "节点" + (i + 1).ToString();
                this.panel1.Controls.Add(label1);
               



                CCWin.SkinControl.SkinLabel label4 = new CCWin.SkinControl.SkinLabel();
                label4.Name = "labelZD4" + i;//
                label4.Parent = this;
                label4.Location = new System.Drawing.Point(605, 33 + i * 90);
                label4.Size = new System.Drawing.Size(69, 17);
                label4.Text = "连接节点";
                label4.Visible = false;
                this.panel1.Controls.Add(label4);
                skinlabellist.Add(label4);
                
              
                CCWin.SkinControl.SkinLabel label2 = new CCWin.SkinControl.SkinLabel();
                label2.Name = "labelZD2" + i;
                label2.Parent = this;
                label2.Location = new System.Drawing.Point(282, 33 + i * 90);
                label2.Size = new System.Drawing.Size(69, 17);
                label2.Text = "";
                this.panel1.Controls.Add(label2);
                skinlabellist1.Add(label2);

                CCWin.SkinControl.SkinLabel label3 = new CCWin.SkinControl.SkinLabel();
                label3.Name = "labelZD3" + i;//Q还是U；
                label3.Parent = this;
                label3.Location = new System.Drawing.Point(440, 33 + i * 90);
                label3.Size = new System.Drawing.Size(69, 17);
                label3.Text = "";
                this.panel1.Controls.Add(label3);
                skinlabellist2.Add(label3);
                

            }
            CCWin.SkinControl.SkinButton Button1 = new CCWin.SkinControl.SkinButton();
            Button1.Name = "Button";
            Button1.Parent = this;
            Button1.Location = new System.Drawing.Point(434, 33 + i * 90);
            Button1.Size = new System.Drawing.Size(75, 23);
            Button1.Text = "提交";
            this.panel1.Controls.Add(Button1);
            Button1.Click += new System.EventHandler(this.skinButton_Click);




            WeiTuoCreated();
            WeiTuoCreated2();
        }
        
        
        ///创建委托，为控件添加事件
        public void WeiTuoCreated()
        {
            foreach (var item in skincomboList)
            {
                item.SelectedIndexChanged += new System.EventHandler(skinCombo_SelectedIndexChanged);
            }

        }

        /// <summary>
        /// 创建委托，为控件添加事件
        /// </summary>
        public void WeiTuoCreated2()
        {
            foreach (var item in skinlabellist)
            {
                item.Click += new System.EventHandler(skinLabel_Click);
            }

        }

        static int biaohao = 0;//定义编号用于传值
        #region 添加事件
        private void skinLabel_Click(object sender, EventArgs e)
        {
            int i = 0;
            CCWin.SkinControl.SkinLabel skinlabel = sender as CCWin.SkinControl.SkinLabel;
            for (i = 0; i < skinlabellist.Count; i++)
            {
                if (skinlabel == skinlabellist[i])
                { break; }
            }
            biaohao = i;
            Form3 fr = new Form3(JieDian,i, ValueSend);
            fr.Show();
        }
        private void skinCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i=0;int j = 0;
            CCWin.SkinControl.SkinComboBox skincombo = sender as CCWin.SkinControl.SkinComboBox;
            for (i = 0; i < skincomboList.Count; i++)
            {
                if (skincombo == skincomboList[i])
                { break; }
            }
            j = skincombo.Location.X;
            if (skincomboList[i].Text == "PQ节点")
            {
                skinlabellist1[i].Text = "P";
                skinlabellist1[i].Visible = true;
                skinlabellist2[i].Text = "Q";
                skinlabellist2[i].Visible = true;
                textboxlist[i].Visible = true;
                textboxlist1[i].Visible = true;
                skinlabellist[i].Visible = true;

            }
            else if (skincomboList[i].Text == "PV节点")
            {
                skinlabellist1[i].Text = "P";
                skinlabellist1[i].Visible = true;
                skinlabellist2[i].Text = "V";
                skinlabellist2[i].Visible = true;
                textboxlist[i].Visible = true;
                textboxlist1[i].Visible = true;
                skinlabellist[i].Visible = true;

            }
            else
            {
                skinlabellist1[i].Text = "U";
                skinlabellist1[i].Visible = true;
                skinlabellist2[i].Text = "Thera";
                skinlabellist2[i].Visible = true;
                textboxlist[i].Visible = true;
                textboxlist1[i].Visible = true;
                skinlabellist[i].Visible = true;
            }

        }
        private void Analyse_Click(object sender, EventArgs e)
        {

        }
        #endregion
        private void Form2_Load(object sender, EventArgs e)
        {
            
        }
        /// <summary>
        /// 打开文本文档获得数据，生成文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void skinButton_Click(object sender, EventArgs e)
        {
            double[,] G1 = new double[JieDian, JieDian];//DP/U
            double[,] B1 = new double[JieDian, JieDian];//DP/U
            for (int i = 0; i < JieDian; i++)
            {
                for (int j = 0; j < JieDian; j++)
                {
                    G1[i, j] = G[i, j];
                    B1[i, j] = B[i, j];
                }
            }
            #region 写入文件
            if (!System.IO.File.Exists("TestTxt.txt"))
            {
                FileStream fs1 = new FileStream("TestTxt.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1);

                for (int hh = 0; hh < textboxlist.Count; hh++)
                {
                    if (skincomboList[hh].Text == "PQ节点")
                    {

                        if (Convert.ToDouble(textboxlist1[hh].Text.Trim()) < 0)
                        {
                            sw.WriteLine("节点" + (hh + 1).ToString() + "：" + skincomboList[hh].Text + "，" + " " + "S(" + (hh + 1).ToString() + ")=" + textboxlist[hh].Text + "-j" + -Convert.ToDouble(textboxlist1[hh].Text.Trim()));// 开始写入值

                        }
                        else
                        {
                            sw.WriteLine("节点" + (hh + 1).ToString() + "：" + skincomboList[hh].Text + "，" + " " + "S(" + (hh + 1).ToString() + ")=" + textboxlist[hh].Text + "+j" + textboxlist1[hh].Text);//开始写入值

                        }
                    }
                    else if (skincomboList[hh].Text == "PV节点")
                    {
                        sw.WriteLine("节点" + (hh + 1).ToString() + "：" + skincomboList[hh].Text + "，" + " " + "P(" + (hh + 1).ToString() + ")=" + textboxlist[hh].Text + " " + "Q(" + (hh + 1).ToString() + ")=" + textboxlist1[hh].Text);//开始写入值
                    }
                    else
                    {
                        sw.WriteLine("节点" + (hh + 1).ToString() + "：" + skincomboList[hh].Text + "，" + " " + "U(" + (hh + 1).ToString() + ")=" + textboxlist[hh].Text + "∠" + textboxlist1[hh].Text);//开始写入值
                    }
                }

                sw.WriteLine("导纳矩阵：");
                string ffa = null;

                for (int i = 0; i < JieDian; i++)
                {
                    for (int j = 0; j < JieDian; j++)
                    {
                        if (B1[i, j] < 0)
                        {
                            string[] templ = B1[i, j].ToString().Split(new char[] { '-' });
                            ffa = ffa + G1[i, j] + "-"+"j"+ templ[0];


                        }
                        else
                        {
                            string[] templ = B1[i, j].ToString().Split(new char[] { '+' });
                            ffa = ffa + G1[i, j] + "+"+"j" + templ[0];
                        }
                        
                         sw.WriteLine(ffa+"     ");
                    }

                }



               
                sw.Close();
                fs1.Close();
            }
            else
            {
                System.IO.File.WriteAllText("TestTxt.txt", "");
                FileStream fs = new FileStream("TestTxt.txt", FileMode.Open, FileAccess.Write);
                StreamWriter sr = new StreamWriter(fs);

                for (int hh = 0; hh < textboxlist.Count; hh++)
                {
                    if (skincomboList[hh].Text == "PQ节点")
                    {

                        if (Convert.ToDouble(textboxlist1[hh].Text.Trim()) < 0)
                        {
                            sr.WriteLine("节点" + (hh + 1).ToString() + "：" + skincomboList[hh].Text + "，" + " " + "S(" + (hh + 1).ToString() + ")=" + textboxlist[hh].Text + "-j" + -Convert.ToDouble(textboxlist1[hh].Text.Trim()));//开始写入值

                        }
                        else
                        {
                            sr.WriteLine("节点" + (hh + 1).ToString() + "：" + skincomboList[hh].Text + "，" + " " + "S(" + (hh + 1).ToString() + ")=" + textboxlist[hh].Text + "+j" + textboxlist1[hh].Text);//开始写入值

                        }
                    }
                    else if (skincomboList[hh].Text == "PV节点")
                    {
                        sr.WriteLine("节点" + (hh + 1).ToString() + "：" + skincomboList[hh].Text + "，" + " " + "P(" + (hh + 1).ToString() + ")=" + textboxlist[hh].Text + " " + "U(" + (hh + 1).ToString() + ")=" + textboxlist1[hh].Text);//开始写入值
                    }
                    else
                    {
                        sr.WriteLine("节点" + (hh + 1).ToString() + "：" + skincomboList[hh].Text + "，" + " " + "U(" + (hh + 1).ToString() + ")=" + textboxlist[hh].Text + "∠" + textboxlist1[hh].Text);//开始写入值
                    }
                }
               sr.WriteLine("导纳矩阵：");
                
                for (int i = 0; i < JieDian; i++)
                {
                    string ffa = null;
                    for (int j = 0; j < JieDian; j++)
                    {
                        if (B1[i, j] < 0)
                        {
                            
                            ffa = ffa + "       " + G1[i, j] + "-" + "j" +( -B1[i, j]).ToString();


                        }
                        else
                        {
                            //string[] templ = B1[i, j].ToString().Split(new char[] { '+' });
                            ffa = ffa +"       "+ G1[i, j] + "+" + "j" + B1[i, j].ToString();
                        }


                    }
                    sr.WriteLine(ffa);
                }


                
              
                sr.Close();
                fs.Close();

            }
            #endregion
            ProcessStartInfo psi = new ProcessStartInfo(@"E:\C#学习\ChaoLiu\PowerMonitor\bin\Debug\TestTxt.txt");
            //创建进程对象
            Process pro = new Process();
            //告诉进程要打开的文件信息
            pro.StartInfo = psi;
            //调用函数打开
            pro.Start();




        }


        double[,] G = new double[100, 100];//只能计算100阶的矩阵
        double[,] B = new double[100, 100];//

        //用于调用委托
        public void ValueSend(List<double> list1, List<double> list2)
        {
          
            for (int l = 0; l < list1.Count; l++)
            {
                G[biaohao, l]= list1[l];
                B[biaohao, l] = list2[l];

            }
        }

        private void Calculate_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
           
            //CalculateEngine.Calculate.abc();
           
            
            panel2.Show();
        }
        
        private void WiFi_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel1.Show();
        }

        private void skinLabel3_Click(object sender, EventArgs e)
        {
        
        }


        #region chart绘制功能，包括使用timer等
        private void skinButton2_Click(object sender, EventArgs e)
        {
            InitChart1();
        }

        private void skinButton7_Click(object sender, EventArgs e)
        {
            InitChart2();
        }





        
        private Queue<double> dataQueue1 = new Queue<double>(10);
        private Queue<double> dataQueue2 = new Queue<double>(10);
       
        private int num = 1;//每次删除增加几个点
        private void UpdateQueueValue1(double du1)
        {

            if (dataQueue1.Count > 10)
            {
                //先出列
                for (int i = 0; i < num; i++)
                {
                    dataQueue1.Dequeue();
                }
            }
           
                
                for (int i = 0; i < num; i++)
                {
                    dataQueue1.Enqueue(du1);
                }
           
        }
        private void UpdateQueueValue2(double dthera1)
        {

            if (dataQueue2.Count > 10)
            {
                //先出列
                for (int i = 0; i < num; i++)
                {
                    dataQueue2.Dequeue();
                }
            }

        
            for (int i = 0; i < num; i++)
            {
                dataQueue2.Enqueue(dthera1);
            }

        }
        private void InitChart1()
        {
            //定义图表区域
            this.chart1.ChartAreas.Clear();
            ChartArea chartArea1 = new ChartArea("C1");
            this.chart1.ChartAreas.Add(chartArea1);
            //定义存储和显示点的容器
            this.chart1.Series.Clear();
            Series series1 = new Series("S1");
            series1.ChartArea = "C1";
            this.chart1.Series.Add(series1);
            //设置图表显示样式
            this.chart1.ChartAreas[0].AxisY.Minimum = -0.5;
            this.chart1.ChartAreas[0].AxisY.Maximum = +0.5;
            this.chart1.ChartAreas[0].AxisX.Interval = 5;
            this.chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            this.chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            //设置标题
            this.chart1.Titles.Clear();
            this.chart1.Titles.Add("S01");
            this.chart1.Titles[0].Text = "电压显示";
            this.chart1.Titles[0].ForeColor = Color.RoyalBlue;
            this.chart1.Titles[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            //设置图表显示样式
            this.chart1.Series[0].Color = Color.Red;
            this.chart1.Series[0].ChartType = SeriesChartType.Line;

            this.chart1.Series[0].Points.Clear();
        }
        private void InitChart2()
        {
            //定义图表区域
            this.chart2.ChartAreas.Clear();
            ChartArea chartArea1 = new ChartArea("C1");
            this.chart2.ChartAreas.Add(chartArea1);
            //定义存储和显示点的容器
            this.chart2.Series.Clear();
            Series series1 = new Series("S1");
            series1.ChartArea = "C1";
            this.chart2.Series.Add(series1);
            //设置图表显示样式
            this.chart2.ChartAreas[0].AxisY.Minimum = -0.5;
            this.chart2.ChartAreas[0].AxisY.Maximum = +0.5;
            this.chart2.ChartAreas[0].AxisX.Interval = 5;
            this.chart2.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            this.chart2.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            //设置标题
            this.chart2.Titles.Clear();
            this.chart2.Titles.Add("S01");
            this.chart2.Titles[0].Text = "角度显示";
            this.chart2.Titles[0].ForeColor = Color.RoyalBlue;
            this.chart2.Titles[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            //设置图表显示样式
            this.chart2.Series[0].Color = Color.Red;
            this.chart2.Series[0].ChartType = SeriesChartType.Line;

            this.chart2.Series[0].Points.Clear();
        }
        static int vvg = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {

            if (vvg < du.Count)
            {
                UpdateQueueValue1(du[vvg]);
                this.chart1.Series[0].Points.Clear();
                for (int i = 0; i < dataQueue1.Count; i++)
                {
                    this.chart1.Series[0].Points.AddXY((i + 1), dataQueue1.ElementAt(i));
                }
            }
            else
            {
                timer1.Stop();
            }
            vvg++;
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            this.timer1.Start();
        }
        static int vvg1 = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (vvg1 < thera.Count)
            {
                UpdateQueueValue2(thera[vvg1]);
                this.chart2.Series[0].Points.Clear();
                for (int i = 0; i < dataQueue2.Count; i++)
                {
                    this.chart2.Series[0].Points.AddXY((i + 1), dataQueue2.ElementAt(i));
                }
            }
            else
            {
                timer2.Stop();
            }
            vvg1++;
        }

        private void skinButton6_Click(object sender, EventArgs e)
        {
            this.timer2.Start();
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            this.timer1.Stop();
        }

        private void skinButton5_Click(object sender, EventArgs e)
        {
            this.timer2.Stop();
        }
        #endregion
        List<double> du = new List<double> { };//为了将数据加载到界面；
        List<double> thera = new List<double> { };
        //调用类库文件进行计算
        private void skinButton8_Click(object sender, EventArgs e)
        {
            Mode.Text = "正忙";
            double[] u11 = new double[JieDian-1];
            double[] thera11 = new double[JieDian-1];
            List<int> uuu = new List<int> { };
            string str = @"E:\C#学习\ChaoLiu\PowerMonitor\bin\Debug\TestTxt.txt";
            CalculateEngine.Calculate.abc(du, thera, str,u11,thera11,uuu);
            for (int hhi = 0; hhi < u11.Length; hhi++)
            {
                panellist1[hhi].Text = ((float) u11[hhi]).ToString()+ "∠" + ((float)thera11[hhi]).ToString();   
              
            }
            panellist1[u11.Length].Text = "平衡节点电压相角";
            for (int jjh = 0; jjh < panellist3.Count; jjh++)
            {
                panellist3[jjh].Text = "节点" +( uuu[jjh]+1).ToString() + " U： ";
            }
            

             Mode.Text = "计算完成！";
            skinChatRichTextBox1.AppendText("计算结果");
            skinChatRichTextBox1.AppendText(DateTime.Now.Date.ToShortDateString()+"\n");
            string logSave = @"C:\Users\Administrator\Desktop\Input0.txt";
            if (!System.IO.File.Exists(logSave))
            {
                MessageBox.Show("该文件不存在！");
            }
            else
            {
                 FileStream fsRead = new FileStream(logSave, FileMode.Open, FileAccess.Read);

                StreamReader read = new StreamReader(fsRead, Encoding.UTF8);
                string strReadline;

                for (int ji = 0; (strReadline = read.ReadLine()) != null; ji++)
                {
                    skinChatRichTextBox1.AppendText(strReadline+"\n");
                }


            }

            }
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skinButton1_Click(object sender, EventArgs e)
        {
            string filePath= Savas();
            if (filePath != null)
            {
                string text = skinChatRichTextBox1.Text;
                FileStream fs = null;
                byte[] array = new UTF8Encoding(true).GetBytes(text);
                fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                fs.Write(array, 0, array.Length);
                fs.Flush();
                MessageBox.Show("保存成功");
            }
            else
            {
                MessageBox.Show("保存失败，请重新选择！");
            }
        }
    }
}
    
   


