using Application.Core.Conditions.Conditions;
using Iterative_methods.DataTypes.Matrix;
using MathLibrary.DataTypes;

namespace Application.Core.Conditions;

public class GaussExcluder
{
    public void Exclude(SparseMatrix globalMatrix, Vector globalVector, FirstCondition condition)
    {
        for (var i = 0; i < condition.Values.Length; i++)
        {
            globalVector[condition.NodesIndexes[i]] = condition.Values[i];
            globalMatrix.Diag[condition.NodesIndexes[i]] = 1d;

            for (var j = globalMatrix.L.RowPtr[condition.NodesIndexes[i]];
                 j < globalMatrix.L.RowPtr[condition.NodesIndexes[i] + 1];
                 j++)
            {
                globalMatrix.L.Values[j] = 0d;
            }

            for (var j = condition.NodesIndexes[i] + 1; j < globalMatrix.Diag.Length; j++)
            {
                var elementIndex = IndexOf(j, condition.NodesIndexes[i], globalMatrix.L.ColumnPtr, globalMatrix.L.RowPtr);

                if (elementIndex == -1) continue;
                globalMatrix.U.Values[elementIndex] = 0;
            }
        }
    }
    
    private int IndexOf(int rowIndex, int columnIndex, int[] ColumnsIndexes, int[] RowsIndexes)
    {
        return  Array.IndexOf(ColumnsIndexes, columnIndex, RowsIndexes[rowIndex],
            RowsIndexes[rowIndex + 1] - RowsIndexes[rowIndex]);
    }
}