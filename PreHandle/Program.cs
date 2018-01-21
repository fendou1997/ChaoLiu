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
            int leaf=0;
            int leaf1 = 0;
            string[] contents = File.ReadAllLines(strFilePath, Encoding.UTF8);
            string balance=null;
            List<string> tempList1 = new List<string> { };
            List<string> tempList2 = new List<string> { };
            List<string> tempList3 = new List<string> { };
            List<int> tem1 = new List<int> { };
            List<int> tem2 = new List<int> { };
            int bal = 0;
            string[,] Daona = new string[100,100];

            #region 读取
            using (FileStream fsRead = new FileStream(strFilePath, FileMode.Open, FileAccess.Read))
            {        
                StreamReader read = new StreamReader(fsRead, Encoding.UTF8);
                string strReadline;
                
                for  (int i=0; (strReadline = read.ReadLine()) != null;i++)
                {
                    string[] temp = strReadline.Split(new char[] { '，', '：' }, StringSplitOptions.RemoveEmptyEntries);
                    if (temp.Length >= 2)
                    {
                        if (temp[1] == "PQ节点")
                        {
                            tempList1.Add(strReadline);
                            tem1.Add(i);
                        }
                        else if (temp[1] == "PV节点")
                        {
                            tempList2.Add(strReadline);
                            tem2.Add(i);
                        }
                        else if (temp[1] == "平衡节点")
                        {
                            balance = strReadline;
                            bal = i;
                        }
                    }
                    else
                    {
                        if (temp[0] == "导纳矩阵")
                        {
                            leaf = i;
                        }
                        else
                        {
                            string[] daona = strReadline.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int jj = 0; jj < daona.Length; jj++)
                            {
                                Daona[i-leaf-1, jj] = daona[jj];
                            }
                            leaf1 = daona.Length;


                        }
                    }
                     
                     
                    // strReadline即为按照行读取的字符串
                }
                string[,] Biaozhun = new string[leaf, leaf];
                for (int n = 0; n < leaf; n++)
                {
                    for (int m = 0; m < leaf; m++)
                    {
                        Biaozhun[n, m] = Daona[n, m];
                    }
                }
                string[,] finalBiaozhun = new string[leaf , leaf ];
                string[,] finalBiaozhun1 = new string[leaf, leaf];
                for (int g = 0; g < tem2.Count; g++)
                {
                    tem1.Add(tem2[g]);
                }
                tem1.Add(bal);
              
                for (int t = 0; t < tem1.Count; t++)
                {
                  
                    for (int x = 0; x < tem1.Count; x++)
                    {
                        finalBiaozhun[t, x] = Biaozhun[tem1[t], x];
                    }

                }
                for (int hh = 0; hh < tem1.Count; hh++)
                {

                    for (int x = 0; x < tem1.Count; x++)
                    {
                        finalBiaozhun1[x, hh] = finalBiaozhun[x, tem1[hh]];
                    }

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
