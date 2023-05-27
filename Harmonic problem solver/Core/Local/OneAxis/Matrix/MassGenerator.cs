using Application.Core.DataTypes;
using Application.DataTypes;
using MathLibrary.DataTypes;

namespace Application.Core.Local;

public class MassGenerator
{
    private readonly MethodData _data;
    private readonly Matrix[] _oneAxisMasses;
    private readonly Matrix _templateMatrix;
    
    public Matrix[] OneAsisMasses => _oneAxisMasses;

    public MassGenerator(MethodData data)
    {
        _templateMatrix = ((double)1 / 6) * (new Matrix(new double[2, 2] { {2, 1}, {1, 2} }));
        _oneAxisMasses = new Matrix[3];
        _data = data;
    }

    public Matrix Generate(IterationData data)
    {
        InitializeOneAxisMatrixes(data);
        Matrix localMass = new Matrix(new double[data.Element.NumberOfIndexes / 2, data.Element.NumberOfIndexes / 2]);
        for (int i = 1; i <= 8; i++)
        {
            for (int j = 1; j <= 8; j++)
            {
                localMass[i - 1, j - 1] = _oneAxisMasses[0][Eta(i) - 1, Eta(j) - 1] * _oneAxisMasses[1][Nu(i) - 1, Nu(j) - 1] * _oneAxisMasses[1][Third(i) - 1, Third(j) - 1];
            }
        }
        
        return localMass;
    }

    private void InitializeOneAxisMatrixes(IterationData data)
    {
        _oneAxisMasses[0] = data.CoordStep[0] * _templateMatrix;
        _oneAxisMasses[1] = data.CoordStep[1] * _templateMatrix;
        _oneAxisMasses[2] = data.CoordStep[2] * _templateMatrix;
    }

    int Eta(int i) => ((i - 1) % 2) + 1;
    int Nu(int i) => ((int)((i - (double)1) / 2) % 2) + 1;
    int Third(int i) => (int)((i - (double)1) / 4) + 1;
}