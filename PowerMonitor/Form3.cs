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
    public delegate void ValueSend(List<double> list1, List<double> list2);
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
        List<CCWin.SkinControl.SkinTextBox> list1 = new List<CCWin.SkinControl.SkinTextBox> { };
        List<CCWin.SkinControl.SkinComboBox> list2 = new List<CCWin.SkinControl.SkinComboBox> { };
        List<System.Windows.Forms.CheckBox> list3 = new List<System.Windows.Forms.CheckBox> { };
        private void Form3_Load(object sender, EventArgs e)
        {
            int i;
            for ( i = 0; i < jieDian; i++)
            {
                if (i != no)
                {
                    CCWin.SkinControl.SkinTextBox textbox = new CCWin.SkinControl.SkinTextBox();
                    textbox.Name = "formtextBox" + i;
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
                    radiobox.Location = new System.Drawing.Point(33, 44 + i * 90);
                    radiobox.Size = new System.Drawing.Size(114, 44);
                    radiobox.Text = "节点" + (i + 1).ToString();


                    this.skinPanel1.Controls.Add(radiobox);
                    list3.Add(radiobox);

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
                    CCWin.SkinControl.SkinTextBox textbox = new CCWin.SkinControl.SkinTextBox();
                    textbox.Name = "formtextBox" + i;
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
            button.Location = new System.Drawing.Point(202, 44 + i * 90);
            button.Size = new System.Drawing.Size(75, 23);
            button.Text = "确认";
           

            this.skinPanel1.Controls.Add(button);
            button.Click += new System.EventHandler(this.skinButton_Click);
            Shijian();
        }
        public void Shijian()
        {
            foreach (var item in list3)
            {
                item.CheckedChanged += new System.EventHandler(SkinCheckBox_CheckedChanged);
            }

        }

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
            }
            else
            {
                list1[i].Visible = false;
                list2[i].Visible = false;

            }



        }

        private void skinButton_Click(object sender, EventArgs e)
        {
            
  
                List<double> list11 = new List<double> { };
                List<double> list12 = new List<double> { };

                for (int i = 0; i < list2.Count; i++)
                {
                    double Re;
                    double Im;
                    string xubu = null;
                    string shibu = null;
                    string temp = null;
                    string[] tempzu;
                if (list3[i].Checked==true && list1[i].Text != "" && list1[i].Text != " " )
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
                else if (list3[i].Checked==false)
                {
                    list11.Add(0);
                    list12.Add(0);
                }
              
                

                }
                Valuelist(list11, list12);


                this.Close();
            }
         
        }
    
}
