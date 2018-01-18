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
using CCWin.SkinControl;
using CCWin.SkinClass;
using CCWin.Win32;
using System.Diagnostics;
using System.Data.SqlClient;

using System.Configuration;


namespace PowerMonitor
{
    public partial class Form1 : CCSkinMain
    {
        private SqlConnection conn = new SqlConnection(ConfigurationSettings.AppSettings["ConnString"]);
        private SqlCommand cmd = new SqlCommand();
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            int H = DateTime.Now.Hour;
            this.Back =
                // H > 5 & H <= 11 ? Resources.morning :     //早上
                // H > 11 & H <= 16 ? Resources.noon :       //中午
                //// H > 16 & H <= 19 ? Resources.afternoon:     //下午
                null;        //晚上
        }




     
        private void btnMima_Click(object sender, EventArgs e)
        {

        }

   
        //登录事件
        Form2 main;
   

      

        private void timShow_Tick_1(object sender, EventArgs e)
        {
            this.Hide();
            tuopan.Text = String.Format("QQ：{0}({1})", txtId.Text, txtId.Text);
            main = new Form2();
            main.Show();
            timShow.Stop();
        }

        private void btnDl_Click_1(object sender, EventArgs e)
        {
            bool leaf;
            leaf = false;


            try
            {
                cmd.Parameters.Clear();
                cmd.Connection = conn;
                cmd.CommandText = "Login";
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                cmd.Parameters.Add("@User", SqlDbType.NChar);
                cmd.Parameters["@User"].Value = txtId.Text;
                cmd.Parameters.Add("@Password", SqlDbType.NChar);
                cmd.Parameters["@Password"].Value = txtPwd.Text;

                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    leaf = true;
                }
                else
                {
                    leaf = false;
                    MessageBox.Show("请输入正确的密码或用户名");
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            if (leaf)
            {
                btnDl.Enabled = btnDuoId.Enabled = btnSandeng.Enabled = false;
                imgLoadding.Visible = true;
                timShow.Start();
            
            }


        }
    }
}
