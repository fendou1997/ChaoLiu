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
            
           



        }
        public static void gonglv(double[] U, double[] thera,double[,]G,double[,] B, double [,] Pgonglv, double[,] Qgonglv,double balanceU, double balancethera)
        {
            double[] UU = new double[U.Length+1];
            for (int k = 0; k < U.Length+1; k++)
            {
                for (int i = 0; i < U.Length+1; i++)
                {
                    if (i != U.Length)
                    {
                        UU[i] = (U[k] * Math.Cos(thera[k]) - U[i] * Math.Cos(thera[i])) * (U[k] * Math.Cos(thera[k]) - U[i] * Math.Cos(thera[i])) + (U[k] * Math.Sin(thera[k]) - U[i] * Math.Sin(thera[i])) * (U[k] * Math.Sin(thera[k]) - U[i] * Math.Sin(thera[i]));
                        Pgonglv[k, i] = G[k, i] * UU[i];
                        Qgonglv[k, i] = B[k, i] * UU[i];
                    }
                    else
                    {
                        UU[i] = (U[k] * Math.Cos(thera[k]) - balanceU * Math.Cos(balancethera)) * (U[k] * Math.Cos(thera[k]) - balanceU * Math.Cos(balancethera)) + (U[k] * Math.Sin(thera[k]) - balanceU * Math.Sin(balancethera)) * (U[k] * Math.Sin(thera[k]) - balanceU * Math.Sin(balancethera));
                        Pgonglv[k, i] = G[k, i] * UU[i];
                        Qgonglv[k, i] = B[k, i] * UU[i];


                    }

                }

            }
            


        }
    }
}
