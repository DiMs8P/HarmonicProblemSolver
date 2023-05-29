using Application.Core.Conditions.Conditions;
using Application.Core.DataTypes;
using MathLibrary.DataTypes;

namespace Application.Core.Conditions;

public class FirstBoundaryConditionProvider
{
    private readonly Grid _grid;
    private readonly Func<Point, double> _uS;
    private readonly Func<Point, double> _uC;
    private readonly PointContainer _points;

    public FirstBoundaryConditionProvider(Grid grid, Func<Point, double> uS, Func<Point, double> uC)
    {
        _points = PointContainer.GetInstance();
        _grid = grid;
        _uS = uS;
        _uC = uC;
    }

    public FirstCondition[] GetConditions(int[] elementsIndexes, Bound[] bounds)
    {
        var conditions = new List<FirstCondition>(elementsIndexes.Length);

        for (var i = 0; i < elementsIndexes.Length; i++)
        {
            var (indexes, _) = _grid.Elements[elementsIndexes[i]].GetBoundNodeIndexes(bounds[i]);

            var complexIndexes = GetComplexIndexes(indexes);
            var values = new double[complexIndexes.Length];

            for (var j = 0; j < indexes.Length; j++)
            {
                values[j * 2] = _uS(_points[indexes[j]]);
                values[j * 2 + 1] = _uC(_points[indexes[j]]);
            }

            conditions.Add(new FirstCondition(complexIndexes, values));
        }

        return conditions.ToArray();
    }

    public FirstCondition[] GetConditions(int elementsByLength, int elementsByWidth, int elementsByHeight)
    {
        var elementsIndexes = new List<int>();
        var bounds = new List<Bound>();

        for (var i = 0; i < elementsByLength * elementsByWidth; i++)
        {
            elementsIndexes.Add(i);
            bounds.Add(Bound.Lower);
        }

        for (var i = 0; i < elementsByHeight; i++)
        {
            for (var j = 0; j < elementsByLength; j++)
            {
                elementsIndexes.Add(i * elementsByWidth * elementsByLength + j);
                bounds.Add(Bound.Front);
            }
        }

        for (var i = 0; i < elementsByHeight; i++)
        {
            for (var j = 0; j < elementsByWidth; j++)
            {
                elementsIndexes.Add(i * elementsByWidth * elementsByLength + j * elementsByLength + (elementsByWidth - 1));
                bounds.Add(Bound.Right);
            }
        }

        for (var i = 0; i < elementsByHeight; i++)
        {
            for (var j = 0; j < elementsByWidth; j++)
            {
                elementsIndexes.Add(i * elementsByWidth * elementsByLength + j * elementsByLength);
                bounds.Add(Bound.Left);
            }
        }

        for (var i = 0; i < elementsByHeight; i++)
        {
            for (var j = 0; j < elementsByWidth; j++)
            {
                elementsIndexes.Add(i * elementsByWidth * elementsByLength + j + elementsByLength * (elementsByWidth - 1));
                bounds.Add(Bound.Back);
            }
        }

        for (var i = elementsByWidth * elementsByLength * (elementsByHeight - 1); i < elementsByWidth * elementsByLength * elementsByHeight; i++)
        {
            elementsIndexes.Add(i);
            bounds.Add(Bound.Upper);
        }

        return GetConditions(elementsIndexes.ToArray(), bounds.ToArray());
    }

    public double GetSValue(int index)
    {
        
        return _uS(_points[index]);
    }

    public double GetCValue(int index)
    {
        return _uC(_points[index]);
    }

    private int[] GetComplexIndexes(int[] indexes)
    {
        var complexIndexes = new int[indexes.Length * 2];

        for (var i = 0; i < indexes.Length; i++)
        {
            complexIndexes[i * 2] = indexes[i] * 2;
            complexIndexes[i * 2 + 1] = indexes[i] * 2 + 1;
        }

        return complexIndexes;
    }
}