using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lianxi
{
    class Program
    {
        static void Main(string[] args)
        {
            
            if (!File.Exists("TestTxt.txt"))
            {
                FileStream fs1 = new FileStream("TestTxt.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1);
                sw.WriteLine("刘招成最帅");//开始写入值
                sw.Close();
                fs1.Close();
            }
            else
            {
                FileStream fs = new FileStream("TestTxt.txt", FileMode.Open, FileAccess.Write);
                StreamWriter sr = new StreamWriter(fs);
                sr.WriteLine("刘招成最帅");//开始写入值
                sr.Close();
                fs.Close();

            }



        }
    }
}
