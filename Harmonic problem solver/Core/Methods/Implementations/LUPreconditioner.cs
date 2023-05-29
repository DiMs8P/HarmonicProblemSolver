using Application.Core.DataTypes.Matrix;
using Iterative_methods.DataTypes.Matrix;
using Iterative_methods.DataTypes.Matrix.Implementations;

namespace Application.Core.Methods.Implementations;

public class LUPreconditioner
{
    public SparseMatrix Decompose(SparseMatrix globalMatrix)
    {
        var preconditionMatrix = globalMatrix.Clone();

        for (var i = 0; i < preconditionMatrix.Diag.Length; i++)
        {
            var sumD = 0.0;
            for (var j = preconditionMatrix.L.RowPtr[i]; j < preconditionMatrix.L.RowPtr[i + 1]; j++)
            {
                var sumL = 0d;
                var sumU = 0d;

                for (var k = preconditionMatrix.L.RowPtr[i]; k < j; k++)
                {
                    var ilPrev = i - preconditionMatrix.L.ColumnPtr[j];
                    var kPrev = IndexOf(i - ilPrev, preconditionMatrix.L.ColumnPtr[k], preconditionMatrix.L.ColumnPtr, preconditionMatrix.L.RowPtr);

                    if (kPrev == -1) continue;
                    
                    sumL += preconditionMatrix.L.Values[k] * preconditionMatrix.U.Values[kPrev];
                    sumU += preconditionMatrix.U.Values[k] * preconditionMatrix.L.Values[kPrev];
                }

                preconditionMatrix.L.Values[j] -= sumL;
                preconditionMatrix.U.Values[j] = (preconditionMatrix.U.Values[j] - sumU) / preconditionMatrix.Diag[preconditionMatrix.L.ColumnPtr[j]];

                sumD += preconditionMatrix.L.Values[j] * preconditionMatrix.U.Values[j];
            }

            preconditionMatrix.Diag[i] -= sumD;
        }

        return preconditionMatrix;
    }

    private int IndexOf(int rowIndex, int columnIndex, int[] ColumnsIndexes, int[] RowsIndexes)
    {
        return  Array.IndexOf(ColumnsIndexes, columnIndex, RowsIndexes[rowIndex],
            RowsIndexes[rowIndex + 1] - RowsIndexes[rowIndex]);
    }
}