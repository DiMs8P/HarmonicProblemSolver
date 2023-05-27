using Iterative_methods.DataTypes.Matrix;
using MathLibrary.DataTypes;

namespace Application.Core.Methods;

public abstract class SlaeSolverMethod
{
    public abstract Vector Solve(SparseMatrixSymmetrical globalMatrix, Vector globalVector);
}