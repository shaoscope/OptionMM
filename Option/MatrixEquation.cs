using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionMM
{
    /// <summary>
    /// 高斯矩阵进行线性拟合
    /// </summary>
    class MatrixEquation
    {
        /// <summary>
        /// 高斯矩阵
        /// </summary>
        private double[,] gaussMatrix;

        /// <summary>
        /// 最高幂次数
        /// </summary>
        private int coe;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MatrixEquation()
        { 

        }   
       
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="arrX">X值列表</param>
        /// <param name="arrY">Y值列表</param>
        /// <param name="n">最高幂次数</param>
        public MatrixEquation(double[] arrX, double[] arrY, int n)
        { 
            coe = n; 
            gaussMatrix = GetGauss(GetXPowSum(arrX, n), GetXPowYSum(arrX, arrY, n), n);
        }

        /// <summary>
        /// 获取高斯矩阵
        /// </summary>
        /// <returns>高斯矩阵</returns>
        public double[,] GetGaussMatrix()
        {
            return gaussMatrix;
        }
      
        /// <summary>
        /// 获取拟合结果 y = a + b * x + c * x^2 + ... + n * x ^ n
        /// </summary>
        /// <returns>数组[a, b, c ... n]</returns>
        public double[] GetResult()
        { 
            return ComputeGauss(gaussMatrix, coe);
        }

        /// <summary>          
        /// 计算获取x散点的幂次和数组         
        /// </summary>          
        /// <param name="arrX">x散点序列</param>         
        /// <param name="n">函数拟合次数</param>         
        /// <returns></returns>          
        protected double[] GetXPowSum(double[] arrX, int n)        
        {              
            int m = arrX.Length;//X散点的个数              
            double[] xPow = new double[2 * n + 1]; //存储X散点的幂次值             
            for (int i = 0; i < xPow.Length; i++)             
            {                  
                if (i == 0)                 
                {                      
                    xPow[i] = m;                 
                }                 
                else 
                {                      
                    //计算x的i次方和                     
                    double max = 0;                      
                    for (int j = 0; j < m; j++)                     
                    {
                        if (arrX[j] == 0)
                        {
                            max = max + 1;
                        }
                        else
                        {
                            max = max + Math.Pow(arrX[j], i);
                        }
                    }                      
                    xPow[i] =Math.Round( max,4);                 
                }             
            }              
            return xPow;         
        }


        /// <summary>          
        /// 计算获取xy的幂次和序列         
        /// </summary>          
        /// <param name="arrX">x散点序列</param>         
        /// <param name="arrY">y散点序列</param>          
        /// <param name="n">拟合曲线次数</param>         
        /// <returns></returns>          
        protected double[] GetXPowYSum(double[] arrX, double[] arrY, int n)         
        {              
            int m = arrX.Length;
            //X散点的个数              
            double[] xyPow = new double[n + 1]; 
            //仓储X散点的幂次值             
            for (int i = 0; i < xyPow.Length; i++)
            {                  
                //计算xy的i次方和                 
                double max = 0;                  
                for (int j = 0; j < m; j++)                 
                {
                    if (arrX[j] == 0)
                    {
                        max = max + 1;
                    }
                    else
                    {
                        max = max + Math.Pow(arrX[j], i) * arrY[j];
                    }
                }                  
                xyPow[i] =Math.Round( max,4);             
            }              
            return xyPow;         
        }

        /// <summary>          
        /// 获取高斯矩阵(增广矩阵)         
        /// </summary>          
        /// <param name="arrX">X的幂次和</param>         
        /// <param name="arrXY">XY的幂次和</param>         
        /// <param name="n">拟合曲线次数</param> 
        /// <returns></returns>          
        protected double[,] GetGauss(double[] arrX, double[] arrXY, int n)         
        {              
            double[,] gauss = new double[n+1, n + 2];             
            for (int i = 0; i < n + 1; i++)             
            {                  
                int j;                 
                int m = i;                  
                for (j = 0; j < n + 1; j++)
                {                      
                    gauss[i, j] = arrX[m];                     
                    m++;                 
                }                  
                gauss[i,j] = arrXY[i];             
            }               
            return gauss;         
        }          

        /// <summary>          
        /// 求解拟合曲线的系数         
        /// </summary>          
        /// <param name="gauss">线性方程的增广矩阵</param>         
        /// <param name="n">方程次数</param>         
        /// <returns></returns>          
        protected double[] ComputeGauss(double[,] gauss, int n)
        {
            double[] a = new double[n + 1]; 
            double s; 
            int matrixLine = n + 1;
            for (int i = 0; i < n + 1; i++)
            {
                a[i] = 0;
            }
            //循环每列              
            for (int j = 0; j < matrixLine; j++)
            {                  
                //每列J行以后的绝对值最大值                 
                double max = 0; 
                int k = j; 
                for (int i = j; i < matrixLine; i++) 
                { 
                    if (Math.Abs(gauss[i, j]) > max) 
                    { 
                        max = gauss[i, j]; 
                        k = i; 
                    }
                }                  
                //判断j行否为最大值行 若不是将j行调换为最大值行                 
                if (k != j)
                {
                    double temp;
                    for (int m = j; m < matrixLine + 1; m++) 
                    { 
                        temp = gauss[j, m]; 
                        gauss[j, m] = gauss[k, m]; 
                        gauss[k, m] = temp;
                    }
                } 
                if (max == 0)
                {                      
                    //奇异矩阵无解                     
                    return a;
                }                  
                //进行初等行变换 得到上三角矩阵                 
                for (int i = j + 1; i < matrixLine; i++) 
                { 
                    s = gauss[i, j]; 
                    for (int m = j; m < matrixLine + 1; m++) 
                    { 
                        gauss[i, m] = Math.Round(gauss[i, m] - gauss[j, m] * s / gauss[j, j], 6); 
                    } 
                }
            }              
            //根据倒推方式一次计算现行方程的解             
            for (int i = matrixLine - 1; i >= 0; i--) 
            { 
                s = 0; 
                for (int j = i + 1; j < matrixLine; j++) 
                { 
                    s += gauss[i, j] * a[j]; 
                } 
                a[i] = Math.Round((gauss[i, matrixLine] - s) / gauss[i, i], 6); 
            }              
            //返回方程的解 即拟合曲线的系数             
            return a;
        }
    }//class
}
