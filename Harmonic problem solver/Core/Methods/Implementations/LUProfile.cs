using Iterative_methods.DataTypes.Matrix;
using MathLibrary.DataTypes;

namespace Application.Core.Methods.Implementations;

public class LUProfile
{
    public Vector Solve(ProfileMatrix globalMatrix, Vector q, Vector b)
    {
        var matrix = globalMatrix.LU();
        var y = CalcY(matrix, q, b);
        var x = CalcX(matrix, y);

        return x;
    }

    private Vector CalcY(ProfileMatrix matrix, Vector q, Vector b)
    {
        var y = q;

        for (var i = 0; i < matrix.CountRows; i++)
        {
            var sum = 0d;

            var k = i - (matrix.RowsIndexes[i + 1] - matrix.RowsIndexes[i]);

            for (var j = matrix.RowsIndexes[i]; j < matrix.RowsIndexes[i + 1]; j++, k++)
            {
                sum += matrix.LowerValues[j] * y[k];
            }

            y[i] = (b[i] - sum) / matrix.Diagonal[i];
        }

        return y;
    }

    private Vector CalcX(ProfileMatrix matrix, Vector y)
    {
        var x = y;

        for (var i = matrix.CountRows - 1; i >= 0; i--)
        {
            var k = i - 1;

            for (var j = matrix.RowsIndexes[i + 1] - 1; j >= matrix.RowsIndexes[i]; j--, k--)
            {
                x[k] -= matrix.UpperValues[j] * x[i];
            }
        }

        return x;
    }
}