using Application.Core.DataTypes;
using Application.Core.Local;
using Application.Core.Local.OneAxis.RightPart;
using Application.DataTypes;
using MathLibrary.DataTypes;

namespace Application.Core.Global;

public class GlobalVectorFiller
{
    private readonly VectorGenerator _vectorGenerator;
    private readonly MassGenerator _massGenerator;

    public GlobalVectorFiller(MethodData methodData)
    {
        _vectorGenerator = new VectorGenerator(methodData);
        _massGenerator = new MassGenerator(methodData);
    }


    public void Fill(Vector globalVector, Grid grid, IterationData iterationData)
    {
        globalVector.Clear();
        PointContainer points = PointContainer.GetInstance();
        foreach (Element element in grid.Element())
        {
            iterationData.Element = element;
            iterationData.CoordStep[0] = points[element[2] / 2][0] - points[element[0] / 2][0];
            iterationData.CoordStep[1] = points[element[4] / 2][1] - points[element[0] / 2][1];
            iterationData.CoordStep[2] = points[element[8] / 2][2] - points[element[0] / 2][2];
            Vector localVector = _vectorGenerator.Generate(_massGenerator.Generate(iterationData), iterationData);
        
            Insert(globalVector, localVector, iterationData);
        }
    }

    private void Insert(Vector globalVector, Vector localVector, IterationData iterationData)
    {
        for (int i = 0; i < iterationData.Element.NumberOfIndexes; i++) 
        {
            globalVector[iterationData.Element[i]] += localVector[i]; 
        }
    }
}