namespace MathLibrary.DataTypes;

public class Matrix
{
    private double[,] _values;

    public Matrix(double[,] values) =>
        _values = values;

    public Matrix(Matrix copyMatrix) =>
        _values = copyMatrix._values.Clone() as double[,];

    public int GetLength(int dimension) => _values.GetLength(dimension);

    public double this[int i, int j]
    {
        get => _values[i, j];
        set => _values[i, j] = value;
    }

    // Operations with Point
    // Operations with Vector
    public static Vector operator *(Matrix matrix, Vector vector)
    {
        if (matrix.GetLength(1) != vector.Size)
        {
            throw new ArgumentException("Matrix and vector with such dimensions cannot be multiplied!");
        }

        double[] output = new double[vector.Size];

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            double sum = 0;
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                sum += matrix[i, j] * vector[j];
            }

            output[i] = sum;
        }

        return new Vector(output);
    }

    // Operations with Matrix
    public static Matrix operator -(Matrix matrix1, Matrix matrix2)
    {
        if (matrix1.GetLength(0) != matrix2.GetLength(0) || matrix1.GetLength(1) != matrix2.GetLength(1))
        {
            throw new ArgumentException("Matrices have different sizes!");
        }

        int rows = matrix1.GetLength(0);
        int cols = matrix1.GetLength(1);

        double[,] newValues = new double[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                newValues[i, j] = matrix1._values[i, j] - matrix2._values[i, j];
            }
        }

        return new Matrix(newValues);
    }

    public static Matrix operator +(Matrix matrix1, Matrix matrix2)
    {
        if (matrix1.GetLength(0) != matrix2.GetLength(0) || matrix1.GetLength(1) != matrix2.GetLength(1))
        {
            throw new ArgumentException("Matrices have different sizes!");
        }

        int rows = matrix1.GetLength(0);
        int cols = matrix1.GetLength(1);

        double[,] newValues = new double[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                newValues[i, j] = matrix1._values[i, j] + matrix2._values[i, j];
            }
        }

        return new Matrix(newValues);
    }

    public static Matrix operator *(Matrix matrix1, Matrix matrix2)
    {
        if (matrix1.GetLength(1) != matrix2.GetLength(0))
        {
            throw new ArgumentException("Matrices can't be multiplied");
        }

        Matrix output = new Matrix(new double[matrix1.GetLength(0), matrix2.GetLength(1)]);

        for (int i = 0; i < matrix1.GetLength(0); i++)
        {
            for (int j = 0; j < matrix2.GetLength(1); j++)
            {
                for (int k = 0; k < matrix2.GetLength(0); k++)
                {
                    output[i, j] += matrix1[i, k] * matrix2[k, j];
                }
            }
        }

        return output;
    }

    // Operations with double
    public static Matrix operator -(Matrix matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        double[,] copyValues = new double[matrix.GetLength(0), matrix.GetLength(1)];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                copyValues[i, j] = -matrix[i, j];
            }
        }

        return new Matrix(copyValues);
    }

    public static Matrix operator *(double multiplier, Matrix matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        double[,] newValues = new double[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                newValues[i, j] = multiplier * matrix._values[i, j];
            }
        }

        return new Matrix(newValues);
    }
}