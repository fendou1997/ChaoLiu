using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CalculateEngine
{
    public class Calculate
    {
        public static void abc()
        {
            /*完成对于文件的输入与输出*/
            string strFilePath = @"C:\Users\Administrator\Desktop\Input.txt";

            string[] contents = File.ReadAllLines(strFilePath, Encoding.UTF8);
            string balance = null;
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
                tempList2.CopyTo(contents, tempList1.Count);
                tempList3.CopyTo(contents, tempList1.Count + tempList2.Count + 1);
                contents[tempList1.Count + tempList2.Count] = balance;
                read.Close();
                fsRead.Close();
            }
            
            #endregion

            #region 写入
            using (FileStream fsRead = new FileStream(strFilePath, FileMode.Open, FileAccess.Write))
            {
                StreamWriter sw = new StreamWriter(fsRead);
                for (int l = 0; l < contents.Length; l++)
                {
                    sw.WriteLine(contents[l]);
                }

                sw.Close();
                fsRead.Close();

            }

            #endregion

            //string fileName = @"C:\Users\Administrator\Desktop\Input.txt";
            //string[] contents = File.ReadAllLines(fileName, Encoding.UTF8);
            List<Data> list = new List<Data> { };//创建一个泛型集合,创建PQ，PV，以及平衡节点
            DataGet(contents, list);
            double[,] G = new double[list.Count, list.Count];
            double[,] B = new double[list.Count, list.Count];
            NetExpand(contents, list, B, G);
            double eps = 0.0001; int j = 0; int i = list.Count - 1; int sumPV = 0;
            foreach (var item in list)
            {
                if (item.Type == "PV节点")
                {
                    sumPV++;
                }
            }
            j = list.Count - sumPV - 1;
            double[] P = new double[i];
            double[] Q = new double[j];///


            double[] U = new double[i];
            double[] thera = new double[i];

            //U[0] = 1; U[1] = 1;
            //U[2] = 1;
            //thera[0] = 0; thera[1] = 0; thera[2] = 0;

            double[] DU = new double[j];
            double[] Dthera = new double[i];///
            //生成B',B"




            double[,] B1 = new double[i, i];
            double[,] B2 = new double[j, j];///



            double[] DP = new double[i];
            double[] DQ = new double[j];///
            double[,] Y1 = new double[i, 1];//DP/U
            double[,] Y2 = new double[j, 1];//DQ/U///
            double[,] X1 = new double[i, 1];//U*DThera
            double[,] X2 = new double[j, 1];//DU///
            //赋值
            //P[0] = -0.4; P[1] = -0.5; P[2] = -0.6;
            //Q[0] = -0.15; Q[1] = -0.20; Q[2] = -0.25;
            int ll = 0;
            foreach (var item in list)
            {

                if (item.Type == "PQ")
                {
                    PQData pq = item as PQData;
                    P[ll] = pq.Positive;
                    Q[ll] = pq.Inpositive;
                    U[ll] = 1;
                    thera[ll] = 0;
                    ll++;
                }
                if (item.Type == "PV")
                {
                    PVData pv = item as PVData;
                    P[ll] = pv.Positive;
                    U[ll] = pv.Volt;
                    thera[ll] = 0;
                    ll++;
                }
            }
            for (int ii = 0; ii < i; ii++)
            {
                for (int iii = 0; iii < i; iii++)
                {
                    B1[ii, iii] = B[ii, iii];
                }
            }
            for (int jj = 0; jj < j; jj++)
            {
                for (int jjj = 0; jjj < j; jjj++)
                {
                    B2[jj, jjj] = B[jj, jjj];
                }
            }

            MainCalculate(i, j, eps, P, Q, G, B, U, thera, DU, Dthera, B1, B2, DP, DQ, Y1, Y2, X1, X2);

        }
        private static void MainCalculate(int i, int j, double eps, double[] P, double[] Q, double[,] G, double[,] B, double[] U, double[] thera, double[] DU, double[] Dthera, double[,] B1, double[,] B2, double[] DP, double[] DQ, double[,] Y1, double[,] Y2, double[,] X1, double[,] X2)
        {
            //设置精度

            for (int h = 0; h < 100000; h++)
            {


                Caculate_Y1(i, U, thera, G, B, P, DP, Y1);

                double[,] dReturn1 = ReverseMatrix(B1, i);
                double[,] dReturn2 = ReverseMatrix(B2, j);


                ChengJi(i, 1, i, dReturn1, Y1, X1);
                for (int o = 0; o < X1.Length; o++)
                {
                    Dthera[o] = X1[o, 0] / U[o];
                }
                for (int c = 0; c < thera.Length; c++)
                {
                    thera[c] = thera[c] + Dthera[c];
                }
                Caculate_Y2(j, U, thera, G, B, Q, DQ, Y2);
                //得到Dthera


                ChengJi(j, 1, j, dReturn2, Y2, X2);//为了得到X2



                for (int o = 0; o < X2.Length; o++)
                {
                    DU[o] = X2[o, 0];
                }
                //判断精度


                if (PanDuan(Dthera, eps) == Dthera.Length && PanDuan(DU, eps) == DU.Length)
                {
                    Console.WriteLine("shu");
                    Console.WriteLine(h);
                    break;
                }
                //修正
                for (int c = 0; c < DU.Length; c++)
                {
                    U[c] = U[c] + DU[c];
                }

            }
        }

        private static int PanDuan(double[] Dthera, double eps)
        {
            int leaf = 0;
            foreach (var item in Dthera)
            {
                if (Math.Abs(item) < eps)
                {
                    leaf = leaf + 1;

                }
            }
            return leaf;
        }

        /// <summary>
        /// 计算矩阵的乘积
        /// </summary>
        /// <param name="n"></param>a行数
        /// <param name="p"></param>b列数
        /// <param name="m"></param>a列数，b行数
        /// <param name="a"></param>A*B
        /// <param name="b"></param>
        /// <param name="c"></param>得到的乘积后的矩阵
        private static void ChengJi(int n, int p, int m, double[,] a, double[,] b, double[,] c)
        {

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < p; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < m; k++)
                    {

                        sum = sum + a[i, k] * b[k, j];
                    }
                    c[i, j] = -sum;


                }
            }
        }

        private static void Caculate_Y2(int j, double[] U, double[] thera, double[,] G, double[,] B2, double[] Q, double[] DQ, double[,] Y2)
        {

            for (int l = 0; l < Q.Length; l++)
            {
                double sum = 0;
                for (int e = 0; e < Math.Sqrt(B2.Length); e++)
                {
                    if (e != Math.Sqrt(B2.Length) - 1)
                    {
                        sum = sum + U[e] * G[l, e] * Math.Sin(thera[l] - thera[e]) - U[e] * B2[l, e] * Math.Cos(thera[l] - thera[e]);
                    }
                    else
                    {
                        sum = sum + 1 * G[l, e] * Math.Sin(thera[l]) - 1 * B2[l, e] * Math.Cos(thera[l]);
                    }

                }
                DQ[l] = Q[l] - U[l] * sum;
                Y2[l, 0] = DQ[l] / U[l];
            }
        }
        public static double MatrixValue(double[,] MatrixList, int Level)  //求得|A| 如果为0 说明不可逆

        {

            //计算行列式的方法
            //   a1 a2 a3
            //  b1 b2 b3
            //  c1 c2 c3
            // 结果为 a1·b2·c3+b1·c2·a3+c1·a2·b3-a3·b2·c1-b3·c2·a1-c3·a2·b1(注意对角线就容易记住了）
            double[,] dMatrix = new double[Level, Level];   //定义二维数组，行列数相同
            for (int i = 0; i < Level; i++)
                for (int j = 0; j < Level; j++)
                    dMatrix[i, j] = MatrixList[i, j];     //将参数的值，付给定义的数组
            double c, x;
            int k = 1;
            for (int i = 0, j = 0; i < Level && j < Level; i++, j++)
            {
                if (dMatrix[i, j] == 0)   //判断对角线上的数据是否为0
                {
                    int m = i;
                    for (; dMatrix[m, j] == 0; m++) ;  //如果对角线上数据为0，从该数据开始依次往后判断是否为0
                    if (m == Level)                      //当该行从对角线开始数据都为0 的时候 返回0
                        return 0;
                    else
                    {
                        // Row change between i-row and m-row
                        for (int n = j; n < Level; n++)
                        {
                            c = dMatrix[i, n];
                            dMatrix[i, n] = dMatrix[m, n];
                            dMatrix[m, n] = c;
                        }
                        // Change value pre-value
                        k *= (-1);
                    }
                }
                // Set 0 to the current column in the rows after current row
                for (int s = Level - 1; s > i; s--)
                {
                    x = dMatrix[s, j];
                    for (int t = j; t < Level; t++)
                        dMatrix[s, t] -= dMatrix[i, t] * (x / dMatrix[i, j]);
                }
            }
            double sn = 1;
            for (int i = 0; i < Level; i++)
            {
                if (dMatrix[i, i] != 0)
                    sn *= dMatrix[i, i];
                else
                    return 0;
            }
            return k * sn;
        }
        public static double[,] ReverseMatrix(double[,] dMatrix, int Level)
        {
            double dMatrixValue = MatrixValue(dMatrix, Level);
            if (dMatrixValue == 0) return null;       //A为该矩阵 若|A| =0 则该矩阵不可逆 返回空
            double[,] dReverseMatrix = new double[Level, 2 * Level];
            double x, c;
            // Init Reverse matrix
            for (int i = 0; i < Level; i++)     //创建一个矩阵（A|I） 以对其进行初等变换 求得其矩阵的逆
            {
                for (int j = 0; j < 2 * Level; j++)
                {

                    if (j < Level)
                        dReverseMatrix[i, j] = dMatrix[i, j];   //该 （A|I）矩阵前 Level列为矩阵A  后面为数据全部为0
                    else
                        dReverseMatrix[i, j] = 0;
                }
                dReverseMatrix[i, Level + i] = 1;
                //将Level+1行开始的Level阶 矩阵装换为单位矩阵 （起初的时候该矩阵都为0 现在在把对角线位置装换为1 ）

                //参考http://www.shuxuecheng.com/gaosuzk/content/lljx/wzja/12/12-6.htm
            }
            for (int i = 0, j = 0; i < Level && j < Level; i++, j++)

            {

                if (dReverseMatrix[i, j] == 0)   //判断一行对角线 是否为0

                {

                    int m = i;

                    for (; dMatrix[m, j] == 0; m++) ;

                    if (m == Level)

                        return null;  //某行对角线为0的时候 判断该行该数据所在的列在该数据后 是否为0 都为0 的话不可逆 返回空值

                    else

                    {

                        // Add i-row with m-row

                        for (int n = j; n < 2 * Level; n++)   //如果对角线为0 则该i行加上m行 m行为（初等变换要求对角线为1，0-->1先加上某行，下面在变1）

                            dReverseMatrix[i, n] += dReverseMatrix[m, n];

                    }

                }

                //  此时数据： 第二行加上第一行为第一行的数据

                //    1   1   3      1    1    0

                //    1   0   1      0    1    0

                //    4   2   1      0    0    1

                //

                // Format the i-row with "1" start

                x = dReverseMatrix[i, j];

                if (x != 1)                  //如果对角线元素不为1  执行以下

                {

                    for (int n = j; n < 2 * Level; n++)

                        if (dReverseMatrix[i, n] != 0)

                            dReverseMatrix[i, n] /= x;   //相除  使i行第一个数字为1

                }

                // Set 0 to the current column in the rows after current row

                for (int s = Level - 1; s > i; s--)         //该对角线数据为1 时，这一列其他数据 要转换为0

                {

                    x = dReverseMatrix[s, j];

                    // 第一次时

                    //    1      1   3      1    1    0

                    //    1      0   1      0    1    0

                    //   4(x)   2   1      0    0    1

                    //

                    for (int t = j; t < 2 * Level; t++)

                        dReverseMatrix[s, t] -= (dReverseMatrix[i, t] * x);

                    //第一个轮回   用第一行*4 减去第三行 为第三行的数据  依次类推

                    //     1      1   3      1    1    0

                    //    1      0   1      0    1    0

                    //    0(x)   -2  -11    -4   -4   1



                }

            }

            // Format the first matrix into unit-matrix

            for (int i = Level - 2; i >= 0; i--)

            //处理第一行二列的数据 思路如上 就是把除了对角线外的元素转换为0 

            {

                for (int j = i + 1; j < Level; j++)

                    if (dReverseMatrix[i, j] != 0)

                    {

                        c = dReverseMatrix[i, j];

                        for (int n = j; n < 2 * Level; n++)

                            dReverseMatrix[i, n] -= (c * dReverseMatrix[j, n]);

                    }

            }

            double[,] dReturn = new double[Level, Level];

            for (int i = 0; i < Level; i++)

                for (int j = 0; j < Level; j++)

                    dReturn[i, j] = dReverseMatrix[i, j + Level];

            //就是把Level阶的矩阵提取出来（减去原先为单位矩阵的部分）

            return dReturn;

        }



        private static void Caculate_Y1(int i, double[] U, double[] thera, double[,] G, double[,] B1, double[] P, double[] DP, double[,] Y1)
        {
            for (int l = 0; l < P.Length; l++)
            {
                double sum = 0;
                for (int e = 0; e < P.Length + 1; e++)
                {
                    if (e != P.Length)
                    {
                        sum = sum + U[e] * G[l, e] * Math.Cos(thera[l] - thera[e]) + U[e] * B1[l, e] * Math.Sin(thera[l] - thera[e]);
                    }
                    else
                    {
                        sum = sum + 1 * G[l, e] * Math.Cos(thera[l]) + 1 * B1[l, e] * Math.Sin(thera[l]);
                    }
                }
                DP[l] = P[l] - U[l] * sum;
                Y1[l, 0] = DP[l] / U[l];
            }
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
                            string[] pqstr = temp[3].Split(new char[] { 'j' }, StringSplitOptions.RemoveEmptyEntries);
                            //

                            list.Add(new PQData { Number = i + 1, Type = "PQ", Positive = Convert.ToDouble(pqstr[0]), Inpositive = Convert.ToDouble(pqstr[1]) });
                        }
                        catch (Exception)
                        {
                        }
                        finally
                        {
                            char[] str = temp[3].ToCharArray();

                            string[] pqstr = temp[3].Split(new char[] { 'j', '-' }, StringSplitOptions.RemoveEmptyEntries);
                            //
                            if (str[0] == '-')
                            {
                                list.Add(new PQData { Number = i + 1, Type = "PQ", Positive = -Convert.ToDouble(pqstr[0]), Inpositive = -Convert.ToDouble(pqstr[1]) });
                            }
                            else
                            {
                                list.Add(new PQData { Number = i + 1, Type = "PQ", Positive = Convert.ToDouble(pqstr[0]), Inpositive = -Convert.ToDouble(pqstr[1]) });
                            }


                        }
                        break;
                    case "PV节点":

                        //

                        list.Add(new PVData { Number = i + 1, Type = "PV", Positive = Convert.ToDouble(temp[3]), Volt = Convert.ToDouble(temp[5]) });
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

            string[] range = contents[contents.Length - 1].Split(new char[] { ' ', '：', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            for (int l = 0; l < list.Count(); l++)
            {
                for (int h = 0; h < list.Count(); h++)
                {

                    string[] temp = range[l * (list.Count()) + h + 1].Split(new char[] { 'j' }, StringSplitOptions.RemoveEmptyEntries);
                    char[] str = temp[0].ToCharArray();
                    string strObj = null;
                    for (int s = 0; s < str.Length - 1; s++)
                    {
                        strObj = strObj + str[s];
                    }
                    G[l, h] = Convert.ToDouble(strObj);


                    if (temp.Length > 1)
                    {
                        if (str[str.Length - 1] == '-')
                        {
                            B[l, h] = -Convert.ToDouble(temp[1]);
                        }
                        else
                        {
                            B[l, h] = -Convert.ToDouble(temp[1]);
                        }

                    }
                    //Console.WriteLine(G[l, h]);
                    //Console.WriteLine(B[l, h]);




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
