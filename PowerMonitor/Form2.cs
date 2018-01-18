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
        private void Open_Click(object sender, EventArgs e)
        {
            string strFilePath = OpenFile();
        }
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

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strSave = Savas();
        }
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
        //----------------------------------------------------------------------------------------------------------//
    }
   

}
