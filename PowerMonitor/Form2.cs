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

namespace PowerMonitor
{
   

    public partial class Form2 : Skin_DevExpress
    {
        public Form2()
        {
            InitializeComponent();
        }
        public int JieDian;//获得节点数
        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_Click(object sender, EventArgs e)
        {
            string strFilePath = OpenFile();
        }
        /// <summary>
        /// 调用打开函数
        /// </summary>
        /// <returns></returns>
        public string OpenFile()
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
            Saveas.Filter = ".ncepu|*.ncepu";//设置文件的类型
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
        List<CCWin.SkinControl.SkinLabel> skinlabellist1 = new List<CCWin.SkinControl.SkinLabel> { };//P||U
        List<CCWin.SkinControl.SkinLabel> skinlabellist2 = new List<CCWin.SkinControl.SkinLabel> { };//P||U

        public void DTCreate(int JieDian)
        {
            for ( int i = 0; i < JieDian; i++)
            {
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
                
                //private void skinCombo1_SelectedIndexChanged(object sender, EventArgs e)
                //  { }
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
                WeiTuoCreated();
                  WeiTuoCreated2();
        }
        public void WeiTuoCreated()
        {
            foreach (var item in skincomboList)
            {
                item.SelectedIndexChanged += new System.EventHandler(skinCombo_SelectedIndexChanged);
            }

        }
        public void WeiTuoCreated2()
        {
            foreach (var item in skinlabellist)
            {
                item.Click += new System.EventHandler(skinLabel_Click);
            }

        }
        private void skinLabel_Click(object sender, EventArgs e)
        {

            Form3 fr = new Form3(JieDian);
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

        private void Form2_Load(object sender, EventArgs e)
        {
           
        }

        //private void skinLabel1_Click(object sender, EventArgs e)
        //{

        //}
    }
}
    
   


