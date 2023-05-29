using Application.Core.DataTypes.Calculus;
namespace Application.Core.DataTypes;

public class Element
{
    public int NumberOfIndexes => _indexes.Length;
    public ReadOnlySpan<int> Indexes => _indexes;
    
    private int[] _indexes;
    private BaseFunction[] _functions;

    public Element(int[] indexes, BaseFunction[] functions)
    {
        _indexes = indexes;
        _functions = functions;
    }
    
    public new int this[int index] => _indexes[index];
    
    public (int[], double[]) GetBoundNodeIndexes(Bound bound) =>
        bound switch
        {
            Bound.Lower =>
                (new[]
                    {
                        _indexes[0] / 2,
                        _indexes[2] / 2,
                        _indexes[4] / 2,
                        _indexes[6] / 2
                    },
                    new[]
                    {
                        GetLenght(),
                        GetWidth()
                    }),
            Bound.Front =>
                (new[]
                    {
                        _indexes[0] / 2,
                        _indexes[2] / 2,
                        _indexes[8] / 2,
                        _indexes[10] / 2
                    },
                    new[]
                    {
                        GetLenght(),
                        GetHeight()
                    }),
            Bound.Back =>
                (new[]
                    {
                        _indexes[4] / 2,
                        _indexes[6] / 2,
                        _indexes[12] / 2,
                        _indexes[14] / 2
                    },
                    new[]
                    {
                        GetLenght(),
                        GetHeight()
                    }),
            Bound.Left =>
                (new[]
                    {
                        _indexes[0] / 2,
                        _indexes[4] / 2,
                        _indexes[8] / 2,
                        _indexes[12] / 2
                    },
                    new[]
                    {
                        GetWidth(),
                        GetHeight()
                    }),
            Bound.Right =>
                (new[]
                    {
                        _indexes[2] / 2,
                        _indexes[6] / 2,
                        _indexes[10] / 2,
                        _indexes[14] / 2
                    },
                    new[]
                    {
                        GetWidth(),
                        GetHeight()
                    }),
            Bound.Upper =>
                (new[]
                    {
                        _indexes[8] / 2,
                        _indexes[10] / 2,
                        _indexes[12] / 2,
                        _indexes[14] / 2
                    },
                    new[]
                    {
                        GetLenght(),
                        GetWidth()
                    }),
            _ => throw new ArgumentOutOfRangeException()
        };

    private double GetLenght()
    {
        PointContainer points = PointContainer.GetInstance();
        return points[_indexes[2] / 2][0] - points[_indexes[0] / 2][0];
    }
    
    private double GetWidth()
    {
        PointContainer points = PointContainer.GetInstance();
        return points[_indexes[4] / 2][1] - points[_indexes[0] / 2][1];
    }
    
    private double GetHeight()
    {
        PointContainer points = PointContainer.GetInstance();
        return points[_indexes[8] / 2][2] - points[_indexes[0] / 2][2];
    }
}