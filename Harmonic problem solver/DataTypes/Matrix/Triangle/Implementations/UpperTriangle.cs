using Application.Core;
using Application.Core.DataTypes;
using Application.Core.DataTypes.Matrix;

namespace Iterative_methods.DataTypes.Matrix.Implementations;

internal class UpperTriangle : Triangle
{
    public UpperTriangle(Grid grid) : base(grid)
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
        _rowPtr = new int[list.Count];
        _rowPtr[0] = list[0].Count;
        for (int i = 1; i < list.Count; i++)
        {
            _rowPtr[i] = _rowPtr[i - 1] + list[i].Count;
        }
    }
}