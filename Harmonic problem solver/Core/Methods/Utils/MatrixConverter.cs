using Iterative_methods.DataTypes.Matrix;

namespace Application.Core.Methods.Utils;

public class MatrixConverter
{
    public static ProfileMatrix Convert(SparseMatrix sparseMatrix)
    {
        var diagonal = sparseMatrix.CloneDiagonal();
        var rowsIndexes = sparseMatrix.CloneRows();
        var lowerValues = new List<double>();
        var upperValues = new List<double>();

        for (var i = 1; i < rowsIndexes.Length; i++)
        {
            var rowBegin = i - 1;

            var j = sparseMatrix.L.RowPtr[i - 1];

            for (; j < sparseMatrix.L.RowPtr[i]; j++)
            {
                if (Math.Abs(sparseMatrix.L.Values[j]) < SlaeParams.Eps
                    && Math.Abs(sparseMatrix.U.Values[j]) < SlaeParams.Eps) continue;
                rowBegin = sparseMatrix.L.ColumnPtr[j];
                break;
            }

            rowsIndexes[i] = rowsIndexes[i - 1] + (i - 1 - rowBegin);

            for (var k = rowsIndexes[i - 1]; k < rowsIndexes[i]; k++, rowBegin++)
            {
                if (sparseMatrix[i - 1, rowBegin] != -1)
                {
                    lowerValues.Add(sparseMatrix.L.Values[IndexOf(i - 1, rowBegin, sparseMatrix.L.ColumnPtr, sparseMatrix.L.RowPtr)]);
                    upperValues.Add(sparseMatrix.U.Values[IndexOf(i - 1, rowBegin, sparseMatrix.L.ColumnPtr, sparseMatrix.L.RowPtr)]);
                }
                else
                {
                    lowerValues.Add(0d);
                    upperValues.Add(0d);
                }
            }
        }

        return new ProfileMatrix(rowsIndexes, diagonal, lowerValues, upperValues);
    }
    
    private static int IndexOf(int rowIndex, int columnIndex, int[] ColumnsIndexes, int[] RowsIndexes)
    {
        return  Array.IndexOf(ColumnsIndexes, columnIndex, RowsIndexes[rowIndex],
            RowsIndexes[rowIndex + 1] - RowsIndexes[rowIndex]);
    }
}