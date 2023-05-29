using Application.Core.DataTypes;
using Application.Core.DataTypes.Matrix;
using Iterative_methods.DataTypes.Matrix.Implementations;

namespace Iterative_methods.DataTypes.Matrix;

public class SparseMatrix : SparseMatrixSymmetrical
{
    public Triangle U => _upperTriangle;
    public Triangle L => _loverTriangle;
    public double[] Diag
    {
        get => _diag;
        set => _diag = value;
    }

    protected Triangle _upperTriangle;
    public SparseMatrix(Grid grid) : base(grid)
    {
        _upperTriangle = new UpperTriangle(grid);
    }
    
    public SparseMatrix(Triangle lowerTriangle, double[] diag, Triangle upperTriangle) : base(lowerTriangle, diag)
    {
        _upperTriangle = upperTriangle;
    }

    public override void Clear()
    {
        base.Clear();
        _upperTriangle.Clear();
    }

    public SparseMatrix Clone()
    {
        Triangle lowerTriangle = new LowerTriangle(L);
        Triangle upperTriangle = new UpperTriangle(U);
        double[] diag = _diag.ToArray();

        return new SparseMatrix(lowerTriangle, diag, upperTriangle);
    }
    
    public int[] CloneRows()
    {
        var rowIndexes = new int[Diag.Length];

        Array.Copy(L.RowPtr, rowIndexes, L.RowPtr.Length);

        return rowIndexes;
    }

    public double[] CloneDiagonal()
    {
        var diagonal = new double[Diag.Length];

        Array.Copy(Diag, diagonal, Diag.Length);

        return diagonal;
    }

    public new IEnumerable<ColumnValue> ColumnValuesByRow(int rowIndex)
    {
        if (rowIndex >= _diag.Length || rowIndex < 0)
        {
            throw new ArgumentException("Out of bounds row index!");
        }

        foreach (var indexValue in _loverTriangle.ColumnValuesByRow(rowIndex))
        {
            yield return indexValue;
        }

        yield return new ColumnValue(rowIndex, _diag[rowIndex]);

        foreach (var indexValue in _upperTriangle.ColumnValuesByRow(rowIndex))
        {
            yield return indexValue;
        }
    }

    public override double this[int i, int j]
    {
        get
        {
            if (i >= 0 && i < _diag.Length && j >= 0 && j < _diag.Length)
            {
                if (i == j)
                {
                    return _diag[i];
                }

                if (i > j)
                {
                    return _loverTriangle[i, j];
                }

                if (i < j)
                {
                    return _upperTriangle[i, j];
                }
            }

            throw new ArgumentException("Wrong indexes!");
        }
        set
        {
            if (i >= 0 && i < _diag.Length && j >= 0 && j < _diag.Length)
            {
                if (i == j)
                {
                    _diag[i] = value;
                }

                if (i > j)
                {
                    _loverTriangle[i, j] = value;
                }

                if (i < j)
                {
                    _upperTriangle[i, j] = value;
                }

                return;
            }

            throw new ArgumentException("Wrong indexes!");
        }
    }
}