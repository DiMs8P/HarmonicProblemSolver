using Application.Core;
using Application.Core.DataTypes;
using Application.Core.DataTypes.Matrix;

namespace Iterative_methods.DataTypes.Matrix.Implementations;

internal class UpperTriangle : Triangle
{
    public UpperTriangle(Grid grid) : base(grid)
    {
    }
    
    public UpperTriangle(Triangle upperTriangle) : base(upperTriangle)
    {
    }
    
    public UpperTriangle(int[] rowPtr, int[] columnPtr, double[] values) : base(rowPtr, columnPtr, values)
    {
    }

    protected override void Initialize(Grid grid)
    {
        PointContainer points = PointContainer.GetInstance();
        
        List<SortedSet<int>> list = new List<SortedSet<int>>();
        for (int i = 0; i < points.Size * 2; i++)
        {
            list.Add(new SortedSet<int>());
        }
    
        foreach (var element in grid.Element())
        {
            for (int i = 0; i < element.NumberOfIndexes; i++)
            {
                for (int j = i + 1; j < element.NumberOfIndexes; j++)
                {
                    list[element.Indexes[i]].Add(element.Indexes[j]);
                }
            }
    
        }
    
        SetIg(list);
        SetJg(list);
        _values = new double[_columnPtr.Length];
    }

    private void SetJg(List<SortedSet<int>> list)
    {
        _columnPtr = list.SelectMany(x => x).ToArray();
    }
    
    private void SetIg(List<SortedSet<int>> list)
    {
        _rowPtr = new int[list.Count + 1];
        _rowPtr[1] = list[0].Count;
        for (int i = 2; i < list.Count + 1; i++)
        {
            _rowPtr[i] = _rowPtr[i - 1] + list[i - 1].Count;
        }
    }
}