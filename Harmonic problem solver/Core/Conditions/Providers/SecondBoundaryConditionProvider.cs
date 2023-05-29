using Application.Core.Conditions.Conditions;
using Application.Core.DataTypes;
using Application.Core.Methods.Utils;
using MathLibrary.DataTypes;

namespace Application.Core.Conditions;

public class SecondBoundaryConditionProvider
{
    public readonly Grid Grid;
    private readonly Matrix _templateMatrix;
    private List<SecondCondition> _conditions = new();
    private readonly PointContainer _points;

    public SecondBoundaryConditionProvider(Grid grid)
    {
        Grid = grid;
        _templateMatrix = GetTemplateMatrix();
        _points = PointContainer.GetInstance();
    }

    private Matrix GetTemplateMatrix()
    {
        return new Matrix(
            new double[,]
            {
                { 4, 0, 2, 0, 2, 0, 1, 0 },
                { 0, 4, 0, 2, 0, 2, 0, 1 },
                { 2, 0, 4, 0, 1, 0, 2, 0 },
                { 0, 2, 0, 4, 0, 1, 0, 2 },
                { 2, 0, 1, 0, 4, 0, 2, 0 },
                { 0, 2, 0, 1, 0, 4, 0, 2 },
                { 1, 0, 2, 0, 2, 0, 4, 0 },
                { 0, 1, 0, 2, 0, 2, 0, 4 }
            }
        );
    }

    public SecondCondition[] GetConditions()
    {
        var conditions = _conditions.ToArray();
        _conditions.Clear();
        return conditions;
    }

    public SecondBoundaryConditionProvider CreateConditions(int[] elementsIndexes, Bound[] bounds,
        Func<Point, double> uS, Func<Point, double> uC)
    {
        var conditions = new List<SecondCondition>(elementsIndexes.Length);

        for (var i = 0; i < elementsIndexes.Length; i++)
        {
            var (indexes, hs) = Grid.Elements[elementsIndexes[i]].GetBoundNodeIndexes(bounds[i]);

            var matrix = GetMatrix(hs[0], hs[1]);

            var vector = GetVector(indexes, uS, uC);
            vector = matrix * CustomMath.Multiply(Config.Lambda, vector);

            var complexIndexes = GetComplexIndexes(indexes);

            conditions.Add(new SecondCondition(new Vector(vector), complexIndexes));
        }

        _conditions.AddRange(conditions);

        return this;
    }

    public Matrix GetMatrix(double h1, double h2)
    {
        return h1 * h2 / 36d * _templateMatrix;
    }

    public int[] GetComplexIndexes(int[] indexes)
    {
        var complexIndexes = new int[indexes.Length * 2];

        for (var i = 0; i < indexes.Length; i++)
        {
            complexIndexes[i * 2] = indexes[i] * 2;
            complexIndexes[i * 2 + 1] = indexes[i] * 2 + 1;
        }

        return complexIndexes;
    }

    private Vector GetVector(int[] indexes, Func<Point, double> uS, Func<Point, double> uC)
    {
        var vector = new Vector(indexes.Length * 2);

        for (var i = 0; i < indexes.Length; i++)
        {
            vector[i * 2] = uS(_points[indexes[i]]);
            vector[i * 2 + 1] = uC(_points[indexes[i]]);
        }

        return vector;
    }
}