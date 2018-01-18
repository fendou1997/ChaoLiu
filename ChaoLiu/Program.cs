using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChaoLiu
{
    class Program
    {
        static void Main(string[] args)
        {
            /*完成对于文件的输入与输出*/
            
            string fileName = @"C:\Users\Administrator\Desktop\Input.txt";
            string[] contents = File.ReadAllLines(fileName, Encoding.UTF8);
            List<Data> list = new List<Data> { };//创建一个泛型集合,创建PQ，PV，以及平衡节点
            DataGet(contents, list);
            double[,] G = new double[list.Count, list.Count];
            double[,] B = new double[list.Count, list.Count];
            NetExpand(contents, list,B,G);

            Console.WriteLine("操作成功");
         
            Console.ReadKey();
        }

        private static void DataGet(string[] contents, List<Data> list)
        {
            for (int i = 0; i < contents.Length; i++)
            {
                string[] temp = contents[i].Split(new char[] { ' ', '，', '=', '∠', '：' }, StringSplitOptions.RemoveEmptyEntries);
                switch (temp[1].Trim())
                {
                    /*节点1：ＰＱ节点，  S(1)=-0.5000-j0.2000
                    节点2：ＰＱ节点，  S(2)=-0.5000-j0.2000
                    节点3：ＰＶ节点，  P(3)=0.4000 V(3)=0.9500
                    节点4：平衡节点，  U(4)=1.0000∠0.0000
                    */

                    case "PQ节点":
                        try
                        {
                            string[] pqstr = contents[i].Split(new char[] { 'j' }, StringSplitOptions.RemoveEmptyEntries);
                            //

                            list.Add(new PQData { Number = i + 1, Type = "PQ", Positive = Convert.ToDouble(pqstr[0]), Inpositive = Convert.ToDouble(pqstr[1]) });
                        }
                        catch (Exception)
                        {
                        }
                        finally
                        {
                            string[] pqstr = contents[i].Split(new char[] { 'j', '-' }, StringSplitOptions.RemoveEmptyEntries);
                            //

                            list.Add(new PQData { Number = i + 1, Type = "PQ", Positive = Convert.ToDouble(pqstr[1]), Inpositive = -Convert.ToDouble(pqstr[2]) });
                        }


                        break;
                    case "PV节点":

                        //

                        list.Add(new PVData { Number = i + 1, Type = "PV", Positive = Convert.ToDouble(temp[3]), Volt = -Convert.ToDouble(temp[5]) });
                        break;
                    case "平衡节点":
                        list.Add(new BalanceData { Number = i + 1, Type = "Balance", Thea = Convert.ToDouble(temp[4]), Volt = Convert.ToDouble(temp[3]) });
                        break;
                    default:

                        break;
                }
            }
        }

        private static void NetExpand(string[] contents, List<Data> list, double[,] B, double[,] G)
        {
          
            string[] range = contents[contents.Length - 1].Split(new char[] { ' ', '：' }, StringSplitOptions.RemoveEmptyEntries);
            for (int l = 0; l < list.Count()-1; l++)
            {
                for (int h = 0; h < list.Count()-1; h++)
                {
                    try
                    {
                        string[] temp = range[l * (list.Count() - 1) + h + 1].Split(new char[] { 'j' }, StringSplitOptions.RemoveEmptyEntries);
                        G[l, h] = Convert.ToDouble(temp[0]);
                        if (temp.Length > 1)
                        {
                            B[l, h] = Convert.ToDouble(temp[1]);
                        }
                        //Console.WriteLine(G[l, h]);
                        //Console.WriteLine(B[l, h]);
                    }
                    catch (Exception)
                    {


                    }
                    finally
                    {
                        string[] temp = range[l * (list.Count() - 1) + h + 1].Split(new char[] { '-', 'j' }, StringSplitOptions.RemoveEmptyEntries);
                        G[l, h] = Convert.ToDouble(temp[0]);
                        if (temp.Length > 1)
                        {
                            B[l, h] = -Convert.ToDouble(temp[1]);
                        }
                        Console.WriteLine(G[l, h]);
                        Console.WriteLine(B[l, h]);

                    }
                   
                }
                
            }
        }

    }
    class Data//使用类进行封装，从而使用泛型集合
    {
       private int number;

        public int Number
        {
            get
            {
                return number;
            }

            set
            {
                number = value;
            }
        }
        private string type;//定义节点类型
        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value.Trim();
            }
        }
     

    }
    class PQData : Data
    {
        private double positive;

        public double  Positive
        {
            get
            {
                return positive;
            }

            set
            {
                positive = value;
            }
        }

        public double Inpositive
        {
            get
            {
                return inpositive;
            }

            set
            {
                inpositive = value;
            }
        }

        private double inpositive;

    }
    class PVData : Data
    {
        private double positive;

        public double Positive
        {
            get
            {
                return positive;
            }

            set
            {
                positive = value;
            }
        }

        public double Volt
        {
            get
            {
                return volt;
            }

            set
            {
                volt = value;
            }
        }

        private double volt;

    }
    class BalanceData : Data
    {

        private double thea;
        public double Volt
        {
            get
            {
                return volt;
            }

            set
            {
                volt = value;
            }
        }

        public double Thea
        {
            get
            {
                return thea;
            }

            set
            {
                thea = value;
            }
        }

        private double volt;

    }
}
