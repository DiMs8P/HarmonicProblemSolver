namespace Application.Core.DataTypes.Matrix;

public abstract class Triangle
{
    public ReadOnlySpan<double> Values => _values;
    public ReadOnlySpan<int> RowPtr => _rowPtr;
    public ReadOnlySpan<int> ColumnPtr => _columnPtr;

    protected int[] _rowPtr = Array.Empty<int>();
    protected int[] _columnPtr = Array.Empty<int>();
    protected double[] _values = Array.Empty<double>();

    protected Triangle(Grid grid) => Initialize(grid);

    public void Clear()
    {
        for (int i = 0; i < _values.Length; i++)
        {
            _values[i] = 0;
        }
    }
    public IEnumerable<ColumnValue> ColumnValuesByRow(int rowIndex)
    {
        if (_rowPtr.Length == 0)
            yield break;

        if (rowIndex < 0) throw new ArgumentOutOfRangeException(nameof(rowIndex));

        var end = _rowPtr[rowIndex];

        var begin = rowIndex == 0
            ? 0
            : _rowPtr[rowIndex - 1];

        for (int i = begin; i < end; i++)
            yield return new ColumnValue(_columnPtr[i], Values[i]);
    }

    protected abstract void Initialize(Grid grid);

    protected double GetValue(int rowIndex, int columnIndex)
    {
        foreach (var columnValue in ColumnValuesByRow(rowIndex))
        {
            if (columnValue.ColumnIndex == columnIndex)
            {
                return columnValue.Value;
            }
        }
        throw new ArgumentException("Wrong triangle indexes!");
    }

    protected void SetValue(int rowIndex, int columnIndex, double value)
    {
        var end = _rowPtr[rowIndex];

        var begin = rowIndex == 0
            ? 0
            : _rowPtr[rowIndex - 1];

        for (int i = begin; i < end; i++)
        {
            if (_columnPtr[i] == columnIndex)
            {
                _values[i] = value;
                return;
            }
        }
        throw new ArgumentException("Wrong triangle indexes!");
    }

    public double this[int i, int j]
    {
        get => GetValue(i, j);
        set => SetValue(i, j, value);
    }

}