using Application.Core.DataTypes;
using Application.Core.Local;
using Application.DataTypes;
using Iterative_methods.DataTypes.Matrix;
using MathLibrary.DataTypes;

namespace Application.Core.Global;

public class GlobalMatrixFiller
{
    private readonly StiffnessGenerator _stiffnessGenerator;
    private readonly MassGenerator _massGenerator;
    private readonly MethodData _methodData;

    public GlobalMatrixFiller(MethodData methodData)
    {
        _stiffnessGenerator = new StiffnessGenerator(methodData);
        _massGenerator = new MassGenerator(methodData);
        _methodData = methodData;
    }

    public void Fill(SparseMatrix outputMatrix, Grid grid, IterationData iterationData)
    {
        outputMatrix.Clear();
        PointContainer points = PointContainer.GetInstance();
        foreach (Element element in grid.Element())
        {
            iterationData.Element = element;
            iterationData.CoordStep[0] = points[element[2] / 2][0] - points[element[0] / 2][0];
            iterationData.CoordStep[1] = points[element[4] / 2][1] - points[element[0] / 2][1];
            iterationData.CoordStep[2] = points[element[8] / 2][2] - points[element[0] / 2][2];
            Matrix localMass = _massGenerator.Generate(iterationData);
            Matrix localStiffness = _stiffnessGenerator.Generate(iterationData, _massGenerator.OneAsisMasses);

            
            Insert(outputMatrix, GetLocalMatrix(localMass, localStiffness, iterationData), iterationData);
        }
    }

    private Matrix GetLocalMatrix(Matrix localStiffness, Matrix localMass, IterationData iterationData)
    {
        Matrix firstThirdMass = (_methodData.Omega * _methodData.Omega * _methodData.Eps) * localMass;
        Matrix secondForthMass = (_methodData.Omega * _methodData.Sigma) * localMass;
        
        Matrix localMatrix =
            new Matrix(new double[iterationData.Element.NumberOfIndexes, iterationData.Element.NumberOfIndexes]);
        for (int i = 0; i < iterationData.Element.NumberOfIndexes / 2; i++)
        {
            for (int j = 0; j < iterationData.Element.NumberOfIndexes / 2; j++)
            {
                localMatrix[i, j] = localStiffness[i, j] - firstThirdMass[i, j];
                localMatrix[i, j + 8] = -secondForthMass[i,j];
                localMatrix[i + 8, j] = secondForthMass[i,j];
                localMatrix[i + 8, j + 8] = localStiffness[i, j] - firstThirdMass[i, j];
            }
        }

        return localMatrix;
    }

    private void Insert(SparseMatrix outputMatrix, Matrix localMatrix, IterationData iterationData)
    {
        for (int i = 0; i < iterationData.Element.NumberOfIndexes; i++) 
        {
            for(int j = 0; j < iterationData.Element.NumberOfIndexes; j++)
            {
                outputMatrix[iterationData.Element[i], iterationData.Element[j]] += localMatrix[i, j];
            }
        }
    }
}