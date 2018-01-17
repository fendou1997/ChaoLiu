using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//对于数据进行计算，达到计算目的。
namespace Calculate
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 3, j = 3;
            //设置初值
            double[] U = new double[i];
            double[] thera = new double[i];

            U[0] = 1; U[1] = 1; U[2] = 1;
            thera[0] = 0; thera[1] = 0; thera[2] = 0;



            double[] DU = new double[i];
            double[] Dthera = new double[i];///
            //生成B',B"
            double[,] G = new double[i, i];
            double[,] B1 = new double[i, i];
            double[,] B2 = new double[i, i];///
            double[] P = new double[i];
            double[] Q = new double[i];///
            double[] DP = new double[i];
            double[] DQ = new double[i];///
            double[,] Y1 = new double[i, 1];//DP/U
            double[,] Y2 = new double[i, 1];//DQ/U///
            double[,] X1 = new double[i, 1];//U*DThera
            double[,] X2 = new double[i, 1];//DU///
            //赋值
            P[0] = -0.4; P[1] = -0.5; P[2] = -0.6;
            Q[0] = -0.15; Q[1] = -0.2; Q[2] = -0.25;
            B1[0, 0] = -10; B1[0, 1] = 0; B1[0, 2] = 0;
            B1[1, 0] = 0; B1[1, 1] = -5.8824; B1[1, 2] = 0;
            B1[2, 0] = 0; B1[2, 1] = 0; B1[2, 2] = -15;
            B2 = B1;
            G[0, 0] = 3.3333; G[0, 1] = 0; G[0, 2] = 0;
            G[1, 0] = 0; G[1, 1] = 1.4706; G[1, 2] = 0;
            G[2, 0] = 0; G[2, 1] = 0; G[2, 2] = 5;
            //设置精度
            double eps = 0.0001;
            for (int h = 0; h < 1000; h++)
            {


                Caculate_Y1(i, U, thera, G, B1, P, DP, Y1);
                Caculate_Y2(i, U, thera, G, B1, Q, DQ, Y2);
                double[,] dReturn1 = ReverseMatrix(B1, i);
                double[,] dReturn2 = ReverseMatrix(B2, i);
                ChengJi(i, 1, i, dReturn1, Y1, X1);
                for (int o = 0; o < X1.Length; o++)
                {
                    Dthera[o] = X1[o, 0] / U[o];
                }
                //得到Dthera
                ChengJi(i, 1, i, dReturn2, Y2, X2);
                for (int o = 0; o < X2.Length; o++)
                {
                    DU[o] = X2[o, 0];
                }
                //判断精度
                if (PanDuan(Dthera, eps) == Dthera.Length && PanDuan(DU, eps) == DU.Length)
                {
                    break;
                }
                //修正
                for (int c = 0; c < U.Length; c++)
                {
                    U[c] = U[c] + DU[c];
                }
                for (int c = 0; c < thera.Length; c++)
                {
                    thera[c] = thera[c] + Dthera[c];
                    if (Math.Abs( thera[c] )> 360)
                    {
                        if (thera[c] < 0)
                        {
                            thera[c] = thera[c] + 360;
                        }
                        if(thera[c] > 0)
                        {
                            thera[c] = thera[c] - 360;
                        }
                    }
                }
            }
            Console.ReadKey();

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
        private static void ChengJi(int n,int p, int m,double [,]a,double [,]b,double [,]c)
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

        private static void Caculate_Y2(int i, double[] U, double[] thera, double[,] G, double[,] B1, double[] Q, double[] DQ, double[,] Y2)
        {
            for (int l = 0; l < Q.Length; l++)
            {
                double sum = 0;
                for (int e = 0; e < Q.Length; e++)
                {
                    sum = sum + U[e] * G[l, e] * Math.Sin(thera[l] - thera[e]) - U[e] * B1[l, e] * Math.Cos(thera[l] - thera[e]);
                }
                DQ[l] = Q[l] - U[l] * sum;
                Y2[l,0] = DQ[l] / U[l];
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
                for (int e = 0; e < P.Length; e++)
                {
                    sum = sum + U[e] * G[l, e] * Math.Cos(thera[l] - thera[e]) + U[e] * B1[l, e] * Math.Sin(thera[l] - thera[e]);
                }
                DP[l] = P[l] - U[l] * sum;
                Y1[l,0] = DP[l] / U[l];
            }
        }
    }
}
