using System.Diagnostics;
using System.Reflection.Emit;

internal class Program
{

    public static double[,] GetMinor(double[,] matrix, int row, int column)
    {
        if (matrix.GetLength(0) != matrix.GetLength(1))
        {
            throw new Exception("The number of rows in the matrix does not match the number of columns");
        }

        double[,] buf = new double[matrix.GetLength(0) - 1, matrix.GetLength(0) - 1];

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if ((i != row) || (j != column))
                {
                    if (i > row && j < column)
                    {
                        buf[i - 1, j] = matrix[i, j];
                    }
                    if (i < row && j > column)
                    {
                        buf[i, j - 1] = matrix[i, j];
                    }
                    if (i > row && j > column)
                    {
                        buf[i - 1, j - 1] = matrix[i, j];
                    }
                    if (i < row && j < column)
                    {
                        buf[i, j] = matrix[i, j];
                    }
                }
            }
        }
        return buf;
    }

    public static double Determ(double[,] matrix)
    {
        if (matrix.GetLength(0) != matrix.GetLength(1))
        {
            throw new Exception("The number of rows in the matrix does not match the number of columns");
        }

        double det = 0;

        int Rank = matrix.GetLength(0);

        if (Rank == 1)
        {
            det = matrix[0, 0];
        }
        if (Rank == 2)
        {
            det = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
        }
        if (Rank > 2)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                det += Math.Pow(-1, 0 + j) * matrix[0, j] * Determ(GetMinor(matrix, 0, j));
            }
        }
        return det;
    }

    private static double[,] GetMatrix(int len)
    {
        double[,] matrix = new double[len, len + 1];

        Console.WriteLine();
        Console.WriteLine("Insert matrix:");

        for (int i = 0; i < len; i++)
        {
            for (int j = 0; j < len; j++)
            {
                bool flag;
                do
                {
                    flag = true;
                    try
                    {
                        Console.Write("Matrix[" + i + ", " + j + "] = ");
                        matrix[i, j] = double.Parse(Console.ReadLine());
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Incorect value, try again");
                        flag = false;
                    }
                }
                while (flag == false);
            }
        }

        Console.WriteLine();
        Console.WriteLine("Insert column of free coefficietns");

        for (int i = 0; i < len; i++)
        {
            bool flag;
            do
            {
                flag = true;
                try
                {
                    Console.Write("K[" + i + "] = ");
                    matrix[i, len] = double.Parse(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine();
                    Console.WriteLine("Incorect value, try again");
                    flag = false;
                }
            }
            while (flag == false);
        }

        return matrix;
    }

    private static void GaussMethod()
    {
        Console.WriteLine();
        Console.WriteLine("Insert matrix size (integer from 1 to 9)");

        int len = 0;
        bool flag;

        do
        {
            flag = true;
            try
            {
                len = int.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Wrong value. Try again");
                flag = false;
            }
            if ((len < 2 || len > 9) && flag == true)
            {
                Console.WriteLine("Wrong size. Try again");
                flag = false;
            }
        }
        while (flag == false);

        double[,] matrix = new double[len, len + 1];
        matrix = GetMatrix(len);

        double[] temp = new double[len + 1];

        int maxi = 0;

        for (int i = 0; i < len - 1; i++)
        {
            for (int j = i; j <= len - 1; j++)
            {
                if (Math.Abs(matrix[j, i]) > Math.Abs(matrix[j, i + 1]))
                {
                    maxi = i + 1;
                }
            }
            for (int j = 0; j < len + 1; j++)
            {
                temp[j] = matrix[i, j];
            }
            for (int j = 0; j < len + 1; j++)
            {
                matrix[i, j] = matrix[maxi, j];
            }
            for (int j = 0; j < len + 1; j++)
            {
                matrix[maxi, j] = temp[j];
            }
        }

        for (int i = 0; i < len; i++)
        {
            if (matrix[i, i] == 0)
            {
                Console.WriteLine();
                Console.WriteLine("System can't be solved using this method");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();

                Main();
            }
        }

        for (int iter = 0; iter < len; iter++)
        {
            double itcoef = matrix[iter, iter];
            for (int i = 0; i < len + 1; i++)
            {
                matrix[iter, i] = matrix[iter, i] / itcoef;
            }

            for (int i = 0; i < len; i++)
            {
                if (i != iter)
                {
                    double coef = matrix[i, iter];
                    double[] subrow = new double[len + 1];

                    if (coef != 0)
                    {
                        for (int j = 0; j < len + 1; j++)
                        {
                            subrow[j] = matrix[iter, j] * coef;
                        }

                        for (int j = 0; j < len + 1; j++)
                        {
                            matrix[i, j] = matrix[i, j] - subrow[j];
                        }
                    }
                }
            }

        }
        Console.WriteLine();
        for (int i = 0; i < len; i++)
        {
            for (int j = 0; j < len + 1; j++)
            {
                Console.Write(Math.Round(matrix[i, j], 3) + " ");
            }
            Console.WriteLine();
        }
    }

    private static void CramerMethod()
    {
        Console.WriteLine();
        Console.WriteLine("Insert matrix size (integer from 1 to 9)");

        int len = 0;
        bool flag;

        do
        {
            flag = true;
            try
            {
                len = int.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Wrong value. Try again");
                flag = false;
            }
            if ((len < 2 || len > 9) && flag == true)
            {
                Console.WriteLine("Wrong size. Try again");
                flag = false;
            }
        }
        while (flag == false);

        double[,] matrix = new double[len, len];
        double[] freedigits = new double[len];
        double[,] submatrix = new double[len, len + 1];

        submatrix = GetMatrix(len);

        for (int i = 0; i < len; i++)
        {
            for (int j = 0; j < len; j++)
            {
                matrix[i, j] = submatrix[i, j];
            }
        }
        for (int i = 0; i < len; i++)
        {
            freedigits[i] = submatrix[i, len];
        }

        double detMain = Determ(matrix);
        if (detMain == 0)
        {
            Console.WriteLine();
            Console.WriteLine("Can't be sloved by Krammer's method");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Main();
        }

        double[] detArray = new double[len];
        double[] helpArray = new double[len];

        for (int i = 0; i < len; i++)
        {
            for (int j = 0; j < len; j++)
            {
                helpArray[j] = matrix[j, i];
                matrix[j, i] = freedigits[j];
            }
            detArray[i] = Determ(matrix);
            for (int j = 0; j < len; j++)
            {
                matrix[j, i] = helpArray[j];
            }
        }

        Console.WriteLine();
        for (int i = 0; i < len; i++)
        {
            for (int j = 0; j < len; j++)
            {
                Console.Write(matrix[i, j] + " ");
            }
            Console.WriteLine();
        }

        Console.WriteLine();
        for (int i = 0; i < len; i++)
        {
            Console.WriteLine("x" + i + " = " + Math.Round(detArray[i] / detMain, 3));
        }

    }

    private static void Main()
    {
        Console.WriteLine("Cource work of the student of the group TIP-71, Alexandrov Semen");
        Console.WriteLine();
        Console.WriteLine("Solving systems of linear equations by Cramer and Jordan-Gauss methods");
        Console.WriteLine();
        Console.WriteLine("Which method do you want to use?");
        Console.WriteLine();
        Console.WriteLine("1. Jordan-Gauss method");
        Console.WriteLine("2. Cramer method");
        Console.WriteLine();

        int choice = 0;
        bool flag = true;

    StartingPoint:

        do
        {
            flag = true;
            try
            {
                choice = Int32.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine();
                Console.WriteLine("Incorrect value, try again");
                flag = false;
            }
        }
        while (flag == false);

        switch(choice)
        {
            case 0:
                Console.Clear();
                Main();
                break;

            case 1:
                GaussMethod();
                break;

            case 2:
                CramerMethod();
                break;

            default:
                Console.WriteLine();
                Console.WriteLine("Incorrect value, try again");
                goto StartingPoint;
        }
    }
}