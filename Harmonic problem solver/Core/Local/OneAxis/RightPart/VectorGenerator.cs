using Application.Core.DataTypes;
using Application.DataTypes;
using MathLibrary.DataTypes;

namespace Application.Core.Local.OneAxis.RightPart;

public class VectorGenerator
{
    private readonly MethodData _methodData;

    public VectorGenerator(MethodData methodData)
    {
        _methodData = methodData;
    }

    public Vector Generate(Matrix localMass, IterationData iterationData)
    {
        PointContainer points = PointContainer.GetInstance();
        Vector ps = new Vector(iterationData.Element.NumberOfIndexes / 2);
        Vector pc = new Vector(iterationData.Element.NumberOfIndexes / 2);

        for (int i = 0; i < iterationData.Element.NumberOfIndexes / 2; i++)
        {
            ps[i] = _methodData.Fs(points[iterationData.Element[2 * i] / 2], iterationData.Time);
            pc[i] = _methodData.Fc(points[iterationData.Element[2 * i] / 2], iterationData.Time);
        }

        Vector bs = localMass * ps;
        Vector bc = localMass * pc;

        Vector localVector = new Vector(iterationData.Element.NumberOfIndexes);

        for (int i = 0; i < localVector.Size / 2; i++)
        {
            localVector[i * 2] = bs[i];
            localVector[i * 2 + 1] = bc[i];
        }

        return localVector;
    }
}