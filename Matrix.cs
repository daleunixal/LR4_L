using System;

namespace LR3
{
    public class Matrix
    {

        public static double Determine(double[,] matrix)
        {
            if (matrix.GetLength(0) != matrix.GetLength(1)) throw new Exception("Not square matrix");
            double det = 0;
            var num = matrix.GetLength(0);
            switch (num)
            {
                case 1:
                    det = matrix[0, 0];
                    break;
                case 2:
                    det = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
                    break;
            }

            if (num <= 2) return det;
            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                det += Math.Pow(-1, 0 + j) * matrix[0, j] * Determine(GetMinor(matrix, 0, j));
            }
            return det;
        }

        public static double[,] GetMinor(double[,] matrix, int row, int column)
        {
            if (matrix.GetLength(0) != matrix.GetLength(1)) throw new Exception("Non square matrix");
            var arr = new double[matrix.GetLength(0) - 1, matrix.GetLength(0) - 1];
            for (var i = 0; i < matrix.GetLength(0); i++)
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    if ((i == row) && (j == column)) continue;
                    if (i > row && j < column) arr[i - 1, j] = matrix[i, j];
                    if (i < row && j > column) arr[i, j - 1] = matrix[i, j];
                    if (i > row && j > column) arr[i - 1, j - 1] = matrix[i, j];
                    if (i < row && j < column) arr[i, j] = matrix[i, j];
                }
            return arr;
        }

        static double[] result;
        public static void ParseMatrix(string[] input)
        {
            double num;
            var n = input.Length;
            var A = new double[n, n];
            var b = new double[n];           
            var flag = true;
            for (var i = 0; i < n; i++)
            {
                var j = 0;
                var s = input[i].Split('=');
                b[i] = Convert.ToDouble(s[1]);
                foreach(var item in s[0])
                {
                    if (item == '-') flag = false;
                    if (!double.TryParse(item.ToString(), out num)) continue;
                    if (flag)
                        A[i, j] = Convert.ToDouble(item.ToString());
                    else
                    {
                        A[i, j] = -Convert.ToDouble(item.ToString());
                        flag = true;
                    }                             
                    j++;
                }
            }
            if (SoLAEInverseMatrix( A, b) == 1)
            {
                Console.WriteLine("System infinity solutions");
                Console.Read();
                return;
            }

            for (var i = 0; i < n; i++)
                Console.WriteLine("x" + i + " = " + result[i]);
            Console.Read();
        }

        public static double[,] MatrixInverse(double[,] matrix)
        {
            var n = matrix.GetLength(0);
            if (Determine(matrix) == 0)
            {
                Console.WriteLine("M^-1 is not exist");
                Console.Read();
                return matrix;
            }

            var result = new double[n, n];
            for (var i = 0; i < n; i++)
            for (var j = 0; j < n; j++)
            {
                result[i, j] = Determine(GetMinor(matrix, i, j)) * Math.Pow(-1, i + j + 2);
            }
            result = Transpose(result, n);
            var det = Determine(matrix);
            for (var i = 0; i < n; i++)
            for (var j = 0; j < n; j++)
            {
                result[i, j] /= det;
            }
            return result;
        }

        public static double[,] Transpose(double[,] a, int n)
        {
            double tmp;
            for (var i = 0; i < n; i++)            
                for (var j = 0; j < i; j++)
                {
                    tmp = a[i, j];
                    a[i, j] = a[j, i];
                    a[j, i] = tmp;
                }
            return a;
        }
        public static double[] Multiply(double[,] A, double[] B)
        {
            var n = B.Length;
            var res = new double[n];
            for (var i = 0; i < n; i++)            
                for (var j = 0; j < n; j++)                
                    res[i] += (A[i, j] * B[j]);
                            
            return res;
        }
        public static int SoLAEInverseMatrix( double[,] A, double[] b)
        {
            var det = Determine(A);
            if (det == 0) return 1;
            result = Multiply(MatrixInverse(A), b);
            return 0;
        }
    }
}
