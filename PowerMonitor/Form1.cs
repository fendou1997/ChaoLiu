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
    public delegate void DelTest(string str);
    public partial class Form1 : Skin_DevExpress
    {
        private DelTest _del;//存储Form1传送过来的函数
        public Form1(DelTest del)
        {
            _del = del;
            
            InitializeComponent();
            this.skinTextBox1.Text = "";
        }

        private void CreateJieDian_Click(object sender, EventArgs e)
        {
            
            
            this._del( skinTextBox1.Text.Trim());
            this.Close();
        }
    }
}
