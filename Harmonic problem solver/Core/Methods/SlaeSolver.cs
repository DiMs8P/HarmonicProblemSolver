using Iterative_methods.DataTypes.Matrix;
using MathLibrary.DataTypes;

namespace Application.Core.Methods;

public class SlaeSolver
{
    private readonly SlaeSolverMethod _method;

    public SlaeSolver(SlaeSolverMethod method)
    {
        _method = method;
    }

    public Vector Solve(SparseMatrix globalMatrix, Vector globalVector)
    {
        return _method.Solve(globalMatrix, globalVector);
    }
}