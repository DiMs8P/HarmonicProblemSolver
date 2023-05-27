using Application.Core.DataTypes;
using Application.DataTypes;
using MathLibrary.DataTypes;
using Point = System.Drawing.Point;

namespace Application.Core.Local;

public class StiffnessGenerator
{
    private readonly MethodData _data;
    private readonly Matrix[] _oneAxisStiffnesses;
    private readonly Matrix _templateStiffness;

    public StiffnessGenerator(MethodData data)
    {
        _templateStiffness = (new Matrix(new double[2, 2] { {1, -1}, {-1, 1} }));
        _oneAxisStiffnesses = new Matrix[3];
        _data = data;
    }

    public Matrix Generate(IterationData data, Matrix[] oneAxisMasses)
    {
        
        InitializeOneAxisMatrixes(data);
        Matrix localStiffness = new Matrix(new double[data.Element.NumberOfIndexes / 2, data.Element.NumberOfIndexes / 2]);
        for (int i = 1; i <= 8; i++)
        {
            for (int j = 1; j <= 8; j++)
            {
                localStiffness[i - 1, j - 1] = 
                    _oneAxisStiffnesses[0][Eta(i) - 1, Eta(j) - 1] * oneAxisMasses[1][Nu(i) - 1, Nu(j) - 1] * oneAxisMasses[2][Third(i) - 1, Third(j) - 1]
                    + oneAxisMasses[0][Eta(i) - 1, Eta(j) - 1] * _oneAxisStiffnesses[1][Nu(i) - 1, Nu(j) - 1] * oneAxisMasses[2][Third(i) - 1, Third(j) - 1]
                    + oneAxisMasses[0][Eta(i) - 1, Eta(j) - 1] * oneAxisMasses[1][Nu(i) - 1, Nu(j) - 1] * _oneAxisStiffnesses[2][Third(i) - 1, Third(j) - 1];
            }
        }
        
        return _data.Lambda * localStiffness;
    }
    
    private void InitializeOneAxisMatrixes(IterationData data)
    {
        _oneAxisStiffnesses[0] = (1 / data.CoordStep[0]) * _templateStiffness;
        _oneAxisStiffnesses[1] = (1 / data.CoordStep[1]) * _templateStiffness;
        _oneAxisStiffnesses[2] = (1 / data.CoordStep[2]) * _templateStiffness;
    }
    
    int Eta(int i) => ((i - 1) % 2) + 1;
    int Nu(int i) => ((int)((i - (double)1) / 2) % 2) + 1;
    int Third(int i) => (int)((i - (double)1) / 4) + 1;
}