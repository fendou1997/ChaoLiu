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

namespace PowerMonitor
{
    public delegate void ValueSend(List<double> list1, List<double> list2);//声明委托，用于传输数据
    public partial class Form3 : Skin_DevExpress
    {
        public int jieDian;
        public int no;
        public ValueSend Valuelist;
      
        public Form3(int jiedian,int i, ValueSend sendList)
        {
            Valuelist = sendList;
            jieDian = jiedian;
            no = i;
            InitializeComponent();
        }
        List<CCWin.SkinControl.SkinTextBox> list1 = new List<CCWin.SkinControl.SkinTextBox> { };//创建泛型集合，存储数据，将控件对象加入到集合中
        List<CCWin.SkinControl.SkinTextBox> Yt = new List<CCWin.SkinControl.SkinTextBox> { };
        List<CCWin.SkinControl.SkinTextBox> Yt1 = new List<CCWin.SkinControl.SkinTextBox> { };
        List<CCWin.SkinControl.SkinComboBox> list2 = new List<CCWin.SkinControl.SkinComboBox> { };
        List<CCWin.SkinControl.SkinComboBox> list4 = new List<CCWin.SkinControl.SkinComboBox> { };//变压器
        List<System.Windows.Forms.CheckBox> list3 = new List<System.Windows.Forms.CheckBox> { };


        /// <summary>
        /// 通过获得form2的节点数从而进行绘制窗体
        /// 通过代码进行动态创建窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form3_Load(object sender, EventArgs e)
        {
            int i;
            for ( i = 0; i < jieDian; i++)
            {
                if (i != no)
                {
                    #region 创建文本框（利用代码）

                    CCWin.SkinControl.SkinTextBox textbox = new CCWin.SkinControl.SkinTextBox();
                    textbox.Name = "formtextBox" + i;
                    textbox.Parent = this;
                    textbox.Location = new System.Drawing.Point(310, 49 + i * 90);
                    textbox.Size = new System.Drawing.Size(121, 30);
                    textbox.Text = "";
                    textbox.Visible = false;

                    this.skinPanel1.Controls.Add(textbox);
                    list1.Add(textbox);
                    #endregion

                    #region 创建选择框
                    CCWin.SkinControl.SkinComboBox combo = new CCWin.SkinControl.SkinComboBox();
                    combo.Name = "formcom" + (i + 1).ToString();
                    combo.Parent = this;
                    combo.Location = new System.Drawing.Point(159, 55 + i * 90);
                    combo.Size = new System.Drawing.Size(121, 28);
                    combo.Visible = false;
                    combo.Items.Add("阻抗");
                    combo.Items.Add("导纳");

                    this.skinPanel1.Controls.Add(combo);
                    list2.Add(combo);
                    #endregion

                    #region 创建变压器i文本
                    CCWin.SkinControl.SkinLabel hhh = new CCWin.SkinControl.SkinLabel();
                    hhh.Name = "formcomm" + (i + 1).ToString();
                    hhh.Parent = this;
                    hhh.Location = new System.Drawing.Point(453, 55 + i * 90);
                    hhh.Size = new System.Drawing.Size(69, 17);
                    hhh.Visible = true;
                    hhh.Text = "变压器" + (i + 1).ToString();
                    this.skinPanel1.Controls.Add(hhh);
                    #endregion

                    #region 创建Gt*、Bt*
                    CCWin.SkinControl.SkinLabel hhh1 = new CCWin.SkinControl.SkinLabel();
                    hhh1.Name = "formm" + (i + 1).ToString();
                    hhh1.Parent = this;
                    hhh1.Location = new System.Drawing.Point(667-30, 55 + i * 90);
                    hhh1.Size = new System.Drawing.Size(39, 17);
                    hhh1.Visible = true;
                    hhh1.Text = "Gt*" + (i + 1).ToString();
                    this.skinPanel1.Controls.Add(hhh1);
                    CCWin.SkinControl.SkinLabel hhh2 = new CCWin.SkinControl.SkinLabel();
                    hhh2.Name = "formm" + (i + 1).ToString();
                    hhh2.Parent = this;
                    hhh2.Location = new System.Drawing.Point(667 +90, 55 + i * 90);
                    hhh2.Size = new System.Drawing.Size(39, 17);
                    hhh2.Visible = true;
                    hhh2.Text = "Bt*" + (i + 1).ToString();
                    this.skinPanel1.Controls.Add(hhh2);
                    #endregion


                    #region 创建变压器变比选择项
                    CCWin.SkinControl.SkinComboBox bianyaqi = new CCWin.SkinControl.SkinComboBox();
                    bianyaqi.Name = "formcomm" + (i + 1).ToString();
                    bianyaqi.Parent = this;
                    bianyaqi.Location = new System.Drawing.Point(528, 55 + i * 90);
                    bianyaqi.Size = new System.Drawing.Size(121-30, 28);
                    bianyaqi.Visible = false;
                    bianyaqi.Items.Add("1.05");
                    bianyaqi.Items.Add("1.025");
                    bianyaqi.Items.Add("0");
                    bianyaqi.Items.Add("0.975");
                    bianyaqi.Items.Add("0.95");
                    bianyaqi.SelectedText = "0";
                    this.skinPanel1.Controls.Add(bianyaqi);
                    list4.Add(bianyaqi);

                    #endregion

                    #region 创建Gt的文本框
                    CCWin.SkinControl.SkinTextBox textbox1 = new CCWin.SkinControl.SkinTextBox();
                    textbox1.Name = "formtextBx1" + i;
                    textbox1.Parent = this;
                    textbox1.Location = new System.Drawing.Point(764-30-10-30-10, 55 + i * 90);
                    textbox1.Size = new System.Drawing.Size(59, 28);
                    textbox1.Text = "";
                    textbox1.Visible = false;

                    this.skinPanel1.Controls.Add(textbox1);
                    Yt.Add(textbox1);
                    #endregion

                    #region 创建Bt的文本框
                    CCWin.SkinControl.SkinTextBox textbox2 = new CCWin.SkinControl.SkinTextBox();
                    textbox2.Name = "formtextBox1" + i;
                    textbox2.Parent = this;
                    textbox2.Location = new System.Drawing.Point(764 - 30 - 10 - 30 - 10+140, 55 + i * 90);
                    textbox2.Size = new System.Drawing.Size(59, 28);
                    textbox2.Text = "";
                    textbox2.Visible = false;

                    this.skinPanel1.Controls.Add(textbox2);
                    Yt1.Add(textbox2);
                    #endregion

                    #region 创建节点i的label
                    System.Windows.Forms.CheckBox radiobox = new System.Windows.Forms.CheckBox();
                    radiobox.Name = "formRadioBox" + i;
                    radiobox.Parent = this;
                    radiobox.Text = "";
                    radiobox.Location = new System.Drawing.Point(33, 44 + i * 90);
                    radiobox.Size = new System.Drawing.Size(114, 44);
                    radiobox.Text = "节点" + (i + 1).ToString();


                    this.skinPanel1.Controls.Add(radiobox);
                    list3.Add(radiobox);
                    #endregion


                    //CCWin.SkinControl.SkinLabel label2 = new CCWin.SkinControl.SkinLabel();
                    //label2.Name = "labelZD3" + i;
                    //label2.Parent = this;
                    //label2.Location = new System.Drawing.Point(27, 61 + i * 90);
                    //label2.Size = new System.Drawing.Size(69, 17);
                    //label2.Text = "节点"+i+1;
                    //label2.Visible = true;
                    //this.skinPanel1.Controls.Add(label2);


                }
                else

                {
                    CCWin.SkinControl.SkinComboBox bianyaqi = new CCWin.SkinControl.SkinComboBox();
                    bianyaqi.Name = "formcomm" + (i + 1).ToString();
                    bianyaqi.Parent = this;
                    bianyaqi.Location = new System.Drawing.Point(528, 55 + i * 90);
                    bianyaqi.Size = new System.Drawing.Size(121-30, 28);
                    bianyaqi.Visible = false;
                    bianyaqi.Items.Add("1.05");
                    bianyaqi.Items.Add("1.025");
                    bianyaqi.Items.Add("0");
                    bianyaqi.Items.Add("0.975");
                    bianyaqi.Items.Add("0.95");
                    bianyaqi.SelectedText = "0";
                    bianyaqi.Enabled = false;
                    this.skinPanel1.Controls.Add(bianyaqi);
                    list4.Add(bianyaqi);

                    CCWin.SkinControl.SkinTextBox textbox1 = new CCWin.SkinControl.SkinTextBox();
                    textbox1.Name = "formtextBox1" + i;
                    textbox1.Parent = this;
                    textbox1.Location = new System.Drawing.Point(764-30-10-30-10, 55 + i * 90);
                    textbox1.Size = new System.Drawing.Size(59, 28);
                    textbox1.Text = "";
                    textbox1.Visible = false;
                    textbox1.Enabled = false;
                    this.skinPanel1.Controls.Add(textbox1);
                    Yt.Add(textbox1);
                    CCWin.SkinControl.SkinTextBox textbox2 = new CCWin.SkinControl.SkinTextBox();
                    textbox2.Name = "formtextBox1" + i;
                    textbox2.Parent = this;
                    textbox2.Location = new System.Drawing.Point(764 - 30 - 10 - 30 - 10 + 140, 55 + i * 90);
                    textbox2.Size = new System.Drawing.Size(59, 28);
                    textbox2.Text = "";
                    textbox2.Visible = false;
                    textbox1.Enabled = false;
                    this.skinPanel1.Controls.Add(textbox2);
                    Yt1.Add(textbox2);


                    CCWin.SkinControl.SkinTextBox textbox = new CCWin.SkinControl.SkinTextBox();
                    textbox.Name = "formteooxtBox" + i;
                    textbox.Parent = this;
                    textbox.Location = new System.Drawing.Point(310, 49 + i * 90);
                    textbox.Size = new System.Drawing.Size(121, 30);
                    textbox.Text = "";
                    textbox.Visible = false;
                    
                    this.skinPanel1.Controls.Add(textbox);
                    list1.Add(textbox);

                    CCWin.SkinControl.SkinComboBox combo = new CCWin.SkinControl.SkinComboBox();
                    combo.Name = "formcom" + (i + 1).ToString();
                    combo.Parent = this;
                    combo.Location = new System.Drawing.Point(159, 55 + i * 90);
                    combo.Size = new System.Drawing.Size(121, 28);
                    combo.Visible = false;
                    combo.Items.Add("阻抗");
                    combo.Items.Add("导纳");

                    this.skinPanel1.Controls.Add(combo);
                    list2.Add(combo);

                    System.Windows.Forms.CheckBox radiobox = new System.Windows.Forms.CheckBox();
                    radiobox.Name = "formRadioBox" + i;
                    radiobox.Parent = this;
                    radiobox.Text = "";
                    radiobox.ForeColor = Color.Red;
                    radiobox.Location = new System.Drawing.Point(33, 44 + i * 90);
                    radiobox.Size = new System.Drawing.Size(114, 44);
                    radiobox.Text = "自导纳或自阻抗";
                   

                    this.skinPanel1.Controls.Add(radiobox);
                    list3.Add(radiobox);



                }
               

            }
            CCWin.SkinControl.SkinButton button = new CCWin.SkinControl.SkinButton();
            button.Name = "formBox";
            button.Parent = this;
            button.Location = new System.Drawing.Point(202+100*2, 44 + i * 90);
            button.Size = new System.Drawing.Size(75, 23);
            button.Text = "确认";
           

            this.skinPanel1.Controls.Add(button);
            button.Click += new System.EventHandler(this.skinButton_Click);
            Shijian();
        }
        
        
        /// <summary>
        /// 为多选框创建事件，从而达到触发的目的
        /// </summary>
        public void Shijian()
        {
            foreach (var item in list3)
            {
                item.CheckedChanged += new System.EventHandler(SkinCheckBox_CheckedChanged);//添加了一个状态改变的事件
            }

        }
        /// <summary>
        ///  public void Shijian()的处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SkinCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            int i=0, j=0;

            System.Windows.Forms.CheckBox skinRadio = sender as System.Windows.Forms.CheckBox;

            for (i = 0; i < list3.Count; i++)
            {
                if (skinRadio == list3[i])
                { break; }
            }
            if (skinRadio.Checked)
            {
                list1[i].Visible = true;
                list2[i].Visible = true;
                list4[i].Visible = true;
                Yt[i].Visible = true;
                Yt1[i].Visible = true;
                Yt[no].Visible = false;
                Yt1[no].Visible = false;
                list4[no].Visible = false;
            }
            else
            {
                list1[i].Visible = false;
                list2[i].Visible = false;
                list4[i].Visible = false;
                Yt[i].Visible = false;
                Yt1[i].Visible = false;
            }



        }

        /// <summary>
        /// 进行导纳矩阵的修改，由于添加变压器则会进行改变导纳矩阵
        /// </summary>
        /// <param name="G"></param>
        /// <param name="B"></param>
        public void YtCreate(double []G,double[]B)
        {
            for (int i = 0; i < Yt.Count; i++)
            {
                if (Yt[i].Text == "")
                {
                    Yt[i].Text = 0.ToString();
                }                
            }
            double[] DG = new double[Yt.Count];
            double[] DB = new double[Yt.Count];
            double sum1 = 0;
            for (int i = 0; i < Yt.Count; i++)
            {
                if (i != no&& (list4[i].Text.Trim())!=null&& Convert.ToDouble(list4[i].Text.Trim())!=0)
                {
                    DG[i] = -Convert.ToDouble(Yt[i].Text.Trim()) * (1 / Convert.ToDouble(list4[i].Text.Trim()) - 1);               
                    sum1=sum1 +Convert.ToDouble(Yt[i].Text.Trim()) * (1 / Convert.ToDouble(list4[i].Text.Trim())* Convert.ToDouble(list4[i].Text.Trim()) - 1);

                }
            }
            double sum2 = 0;
            for (int i = 0; i < Yt1.Count; i++)
            {
                if (i != no && Convert.ToDouble(list4[i].Text.Trim()) != 0)
                {
                    DB[i] = -Convert.ToDouble(Yt1[i].Text.Trim()) * (1 / Convert.ToDouble(list4[i].Text.Trim()) - 1);
                    sum2 = sum2 + Convert.ToDouble(Yt1[i].Text.Trim()) * (1 / Convert.ToDouble(list4[i].Text.Trim()) * Convert.ToDouble(list4[i].Text.Trim()) - 1);

                }
            }
            DB[no] = sum2;

            for (int i = 0; i < Yt.Count; i++)
            {
                G[i] = G[i] + DG[i];
                B[i] = B[i] + DB[i];
            }

        }

        /// <summary>
        /// 添加按钮按下的处理函数，通过委托将数据传给form2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skinButton_Click(object sender, EventArgs e)
        {


            List<double> list11 = new List<double> { };//声明两个list泛型集合
            List<double> list12 = new List<double> { };
            int lleaf = 0;
            foreach (var item in list1)
            {
                string[] tempi = item.Text.Trim().Split(new char[] { 'j' }, StringSplitOptions.RemoveEmptyEntries);
              
            }
            if (lleaf != 0)
            {
                MessageBox.Show("输入错误，输入示例：1+j2或1-j2");
            }
           
                for (int i = 0; i < list2.Count; i++)
                {
                    double Re;
                    double Im;
                    string xubu = null;
                    string shibu = null;
                    string temp = null;
                    string[] tempzu;
                    if (list3[i].Checked == true && list1[i].Text != "" && list1[i].Text != " ")
                    {
                        temp = list1[i].Text.ToString();
                        tempzu = temp.Split(new char[] { 'j' }, StringSplitOptions.RemoveEmptyEntries);
                        char[] abc = tempzu[0].ToArray();
                        if (abc[abc.Length - 1] == '-')
                        {
                            xubu = tempzu[1];
                            Im = -Convert.ToDouble(xubu);
                        }
                        else
                        {
                            xubu = tempzu[1];
                            Im = Convert.ToDouble(xubu);
                        }
                        for (int jj = 0; jj < abc.Length - 1; jj++)
                        {
                            shibu = shibu + abc[jj];
                        }

                        Re = Convert.ToDouble(shibu);
                        if (list2[i].Text == "阻抗")
                        {
                            Re = Re / (Re * Re + Im * Im);
                            Im = -Im / (Re * Re + Im * Im);
                        }
                        else
                        {

                        }



                        list11.Add(Re);
                        list12.Add(Im);
                    }
                    else
                    {
                        list11.Add(0);
                        list12.Add(0);
                    }



                }

                double[] G = new double[list11.Count];
                double[] B = new double[list12.Count];
                G = list11.ToArray();
                B = list12.ToArray();//计算矩阵的阻抗与导纳
                YtCreate(G, B);//进行变压器变比的修改

                Valuelist(G.ToList(), B.ToList());//调用委托，进行传值


                this.Close();//关闭窗口
            
            
            }
         
        }
    
}
