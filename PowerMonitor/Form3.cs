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
   
    public partial class Form3 : Skin_DevExpress
    {
        public int jieDian;
        public Form3(int jiedian)
        {
            jieDian = jiedian;
            InitializeComponent();
        }
        List<CCWin.SkinControl.SkinTextBox> list1 = new List<CCWin.SkinControl.SkinTextBox> { };
        List<CCWin.SkinControl.SkinComboBox> list2 = new List<CCWin.SkinControl.SkinComboBox> { };
        List<System.Windows.Forms.CheckBox> list3 = new List<System.Windows.Forms.CheckBox> { };
        private void Form3_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < jieDian; i++)
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
                combo.Name = "formcom" +( i+1).ToString();
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

   
    }
}
