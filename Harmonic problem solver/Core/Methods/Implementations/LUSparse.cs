using Iterative_methods.DataTypes.Matrix;
using MathLibrary.DataTypes;

namespace Application.Core.Methods.Implementations;

public class LUSparse : SlaeSolverMethod
{
    private readonly LUPreconditioner _luPreconditioner;

    public LUSparse(LUPreconditioner luPreconditioner)
    {
        _luPreconditioner = luPreconditioner;
    }
    public override Vector Solve(SparseMatrix globalMatrix, Vector globalVector)
    {
        var matrix = _luPreconditioner.Decompose(globalMatrix);
        var y = CalcY(matrix, globalVector);
        var x = CalcX(matrix, y);

        return x;
    }
    public Vector CalcY(SparseMatrix sparseMatrix, Vector b)
    {
        var y = b;

        for (var i = 0; i < sparseMatrix.Diag.Length; i++)
        {
            var sum = 0.0;
            for (var j = sparseMatrix.L.RowPtr[i]; j < sparseMatrix.L.RowPtr[i + 1]; j++)
            {
                sum += sparseMatrix.L.Values[j] * y[sparseMatrix.L.ColumnPtr[j]];
            }
            y[i] = (b[i] - sum) / sparseMatrix.Diag[i];
        }

        return y;
    }

    public Vector CalcX(SparseMatrix sparseMatrix, Vector y)
    {
        var vector = new double[sparseMatrix.Diag.Length];
        Array.Copy(y.ToArray(), vector, sparseMatrix.Diag.Length);
        var x = new Vector(vector);

        for (var i = sparseMatrix.Diag.Length - 1; i >= 0; i--)
        {
            for (var j = sparseMatrix.L.RowPtr[i + 1] - 1; j >= sparseMatrix.L.RowPtr[i]; j--)
            {
                x[sparseMatrix.L.ColumnPtr[j]] -= sparseMatrix.U.Values[j] * x[i];
            }
        }

        return x;
    }

    public void CalcXWithoutMemory(SparseMatrix sparseMatrix, Vector y)
    {
        var x = y;

        for (var i = sparseMatrix.Diag.Length - 1; i >= 0; i--)
        {
            for (var j = sparseMatrix.L.RowPtr[i + 1] - 1; j >= sparseMatrix.L.RowPtr[i]; j--)
            {
                x[sparseMatrix.L.ColumnPtr[j]] -= sparseMatrix.U.Values[j] * x[i];
            }
        }
    }
}