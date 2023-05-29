using Iterative_methods.DataTypes.Matrix;
using MathLibrary.DataTypes;

namespace Application.Core.Conditions;

public class Inserter
{
    public void InsertVector(Vector globalVector, Vector localVector, int[] indexes)
    {
        for (var i = 0; i < localVector.Size; i++)
        {
            globalVector[indexes[i]] += localVector[i];
        }
    }
}