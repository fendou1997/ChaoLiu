using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreHandle//用于对文本文档进行处理达到标准化
    //变压器对地支路都进行考虑，留出接口。
{
    class Program
    {
        static void Main(string[] args)
        {
          
            string strFilePath = @"C:\Users\Administrator\Desktop\Input.txt";
            
            string[] contents = File.ReadAllLines(strFilePath, Encoding.UTF8);
            string balance=null;
            List<string> tempList1 = new List<string> { };
            List<string> tempList2 = new List<string> { };
            List<string> tempList3 = new List<string> { };
            #region 读取
            using (FileStream fsRead = new FileStream(strFilePath, FileMode.Open, FileAccess.Read))
            {        
                StreamReader read = new StreamReader(fsRead, Encoding.UTF8);
                string strReadline;
               
                while ((strReadline = read.ReadLine()) != null)
                {
                    string[] temp = strReadline.Split(new char[] { '，', '：' }, StringSplitOptions.RemoveEmptyEntries);
                    switch (temp[1])
                    {
                        case "PQ节点":
                            tempList1.Add(strReadline);
                            break;

                        case "PV节点":
                            tempList2.Add(strReadline);
                            break;
                        case "平衡节点":
                            balance = strReadline;
                            break;
                        default:
                            tempList3.Add(strReadline);
                            break;


                    }    
                    // strReadline即为按照行读取的字符串
                }
                
                tempList1.CopyTo(contents);
                tempList2.CopyTo(contents, tempList1.Count );
                tempList3.CopyTo(contents, tempList1.Count  + tempList2.Count+1);
                contents[tempList1.Count + tempList2.Count] = balance;
                read.Close();
                fsRead.Close();
            }

            #endregion
            #region 写入
            using (FileStream fsRead = new FileStream(strFilePath, FileMode.Open, FileAccess.Write))
            {
                StreamWriter sw = new StreamWriter(fsRead);
                for (int i = 0; i < contents.Length; i++)
                {
                    sw.WriteLine(contents[i]);
                }

                sw.Close();
                fsRead.Close();

            }

            #endregion

        }


    }
}
