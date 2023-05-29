using Iterative_methods.DataTypes.Matrix;
using MathLibrary.DataTypes;

namespace Application.Core.Methods.Utils;

public static class CustomMath
{
    public static double ScalarProduct(Vector vector1, Vector vector2)
    {
        double result = 0;
        for (int i = 0; i < vector1.Size; i++)
        {
            result += vector1[i] * vector2[i];
        }

        return result;
    }
    
    public static Vector Subtract(Vector localVector1, Vector localVector2)
    {
        if (localVector1.Size != localVector2.Size) throw new Exception("Can't Subtract vectors");

        for (var i = 0; i < localVector1.Size; i++)
        {
            localVector2[i] = localVector1[i] - localVector2[i];
        }

        return localVector2;
    }
    
    public static Vector Sum(Vector localVector1, Vector localVector2)
    {
        if (localVector1.Size != localVector2.Size) throw new Exception("Can't Sum vectors");

        for (var i = 0; i < localVector1.Size; i++)
        {
            localVector1[i] += localVector2[i];
        }

        return localVector1;
    }
    
    public static Vector Multiply(double number, Vector vector)
    {
        for (var i = 0; i < vector.Size; i++)
        {
            vector[i] *= number;
        }

        return vector;
    }
    
    public static Vector MultiplyMatrixByVector(SparseMatrix matrix, Vector vector)
    {
        var rowsIndexes = matrix.L.RowPtr;
        var columnsIndexes = matrix.L.ColumnPtr;
        var di = matrix.Diag;
        var lowerValues = matrix.L.Values;
        var upperValues = matrix.U.Values;

        var result = new Vector(matrix.Diag.Length);

        for (var i = 0; i < matrix.Diag.Length; i++)
        {
            result[i] += di[i] * vector[i];

            for (var j = rowsIndexes[i]; j < rowsIndexes[i + 1]; j++)
            {
                result[i] += lowerValues[j] * vector[columnsIndexes[j]];
                result[columnsIndexes[j]] += upperValues[j] * vector[i];
            }
        }

        return result;
    }
}