using Application.Core.Methods.Utils;
using Iterative_methods.DataTypes.Matrix;
using MathLibrary.DataTypes;

namespace Application.Core.Methods.Implementations.LOS;

public class LOS : SlaeSolverMethod
{
    private readonly LUPreconditioner _luPreconditioner;
    private readonly LUSparse _luSparse;
    private SparseMatrix _preconditionMatrix;
    private Vector _r;
    private Vector _z;
    private Vector _p;
    private Vector solution;
    
    public LOS(LUPreconditioner luPreconditioner, LUSparse luSparse)
    {
        _luPreconditioner = luPreconditioner;
        _luSparse = luSparse;
    }
    
    private void PrepareProcess(SparseMatrix globalMatrix, Vector globalVector)
    {
        _preconditionMatrix = _luPreconditioner.Decompose(globalMatrix);
        _r = _luSparse.CalcY(_preconditionMatrix, CustomMath.Subtract(globalVector, CustomMath.MultiplyMatrixByVector(globalMatrix, solution)));
        _z = _luSparse.CalcX(_preconditionMatrix, _r);
        _p = _luSparse.CalcY(_preconditionMatrix, CustomMath.MultiplyMatrixByVector(globalMatrix, _z));
    }


    public override Vector Solve(SparseMatrix globalMatrix, Vector globalVector)
    {
        solution = new Vector(globalVector.Size);
        PrepareProcess(globalMatrix, globalVector);
        return IterationProcess(globalMatrix, globalVector);
    }
    
    private Vector IterationProcess(SparseMatrix globalMatrix, Vector globalVector)
    {
        Console.WriteLine("LOS");
        var iter = 0;
        var residual = CustomMath.ScalarProduct(_r, _r);
        var residualNext = residual;

        for (var i = 1; i <= SlaeParams.MaxIterations && residualNext > Math.Pow(SlaeParams.Eps, 2); i++)
        {
            var scalarPP = CustomMath.ScalarProduct(_p, _p);

            var alpha = CustomMath.ScalarProduct(_p, _r) / scalarPP;

            CustomMath.Sum(solution, alpha * _z);

            var rNext = CustomMath.Subtract(_r, alpha * _p);

            var LAUr = _luSparse.CalcY(_preconditionMatrix, CustomMath.MultiplyMatrixByVector(globalMatrix, _luSparse.CalcX(_preconditionMatrix, rNext)));

            var beta = -(CustomMath.ScalarProduct(_p, LAUr) / scalarPP);

            var zNext = CustomMath.Sum(_luSparse.CalcX(_preconditionMatrix, rNext), CustomMath.Multiply(beta, _z));

            var pNext = CustomMath.Sum(LAUr, CustomMath.Multiply(beta, _p));

            _r = rNext;
            _z = zNext;
            _p = pNext;

            residualNext = CustomMath.ScalarProduct(_r, _r) / residual;
            iter = i;
        }

        Console.WriteLine("кол-во итераций: {0}", iter);

        return solution;
    }
}