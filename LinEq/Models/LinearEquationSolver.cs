using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinEq.Models
{
    public class LinearEquationSolver
    {
        public double[,] matrix;

        public LinearEquationSolver(string equations)
        {
            ParseEquations(equations);
        }

        public void ParseEquations(string equations)
        {
            string[] eqs = equations.Split('|');
            matrix = new double[eqs.Length, eqs[0].Split(',').Length];
            for (int i = 0; i < eqs.Length; i++)
            {
                string[] coeffs = eqs[i].Split(',');
                for (int j = 0; j < coeffs.Length; j++)
                {
                    matrix[i, j] = double.Parse(coeffs[j]);
                }
            }
        }

        public double[] Solve()
        {
            // input checks
            int rowCount = matrix.GetUpperBound(0) + 1;
            if (matrix == null || matrix.Length != rowCount * (rowCount + 1))
                throw new ArgumentException("The algorithm must be provided with a (n x n+1) matrix.");
            if (rowCount < 1)
                throw new ArgumentException("The matrix must at least have one row.");

            // pivoting
            for (int col = 0; col + 1 < rowCount; col++)
            {
                // check for zero coefficients
                if (matrix[col, col] == 0)
                {
                    // find non-zero coefficient
                    int swapRow = col + 1;
                    for (; swapRow < rowCount; swapRow++) 
                        if (matrix[swapRow, col] != 0) 
                            break;

                    if (swapRow <= col)
                    {
                        if (matrix[swapRow, col] != 0) // found a non-zero coefficient?
                        {
                            // yes, then swap it with the above
                            double[] tmp = new double[rowCount + 1];
                            for (int i = 0; i < rowCount + 1; i++)
                            {
                                tmp[i] = matrix[swapRow, i];
                                matrix[swapRow, i] = matrix[col, i];
                                matrix[col, i] = tmp[i];
                            }
                        }
                        else
                            return new double[] {}; // the matrix has no unique solution
                    }
                }
            }

            // elimination
            for (int sourceRow = 0; sourceRow + 1 < rowCount; sourceRow++)
            {
                for (int destRow = sourceRow + 1; destRow < rowCount; destRow++)
                {
                    double df = matrix[sourceRow, sourceRow];
                    double sf = matrix[destRow, sourceRow];
                    for (int i = 0; i < rowCount + 1; i++)
                        matrix[destRow, i] = matrix[destRow, i] * df - matrix[sourceRow, i] * sf;
                }
            }

            // back-insertion
            for (int row = rowCount - 1; row >= 0; row--)
            {
                double f = matrix[row, row];
                if (f == 0)
                    return new double[] { };

                for (int i = 0; i < rowCount + 1; i++) 
                    matrix[row, i] /= f;

                for (int destRow = 0; destRow < row; destRow++)
                { 
                    matrix[destRow, rowCount] -= matrix[destRow, row] * matrix[row, rowCount]; 
                    matrix[destRow, row] = 0; 
                }
            }
            double[] result = new double[rowCount];
            for (int i = 0; i < rowCount; i++)
            {
                result[i] = matrix[i, rowCount];
            }

            return result;
        }
    }
}