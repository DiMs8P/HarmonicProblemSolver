using Application.Core.Methods.Utils;
using Iterative_methods.DataTypes.Matrix;
using MathLibrary.DataTypes;

namespace Application.Core.Methods.Implementations.BSGSTAB;

public class BSGSTAB : SlaeSolverMethod
{
    private readonly LUPreconditioner _luPreconditioner;
    private SparseMatrix _preconditionMatrix;
    private readonly LUSparse _luSparse;
    private Vector _r0;
    private Vector _r;
    private Vector _z;
    private Vector solution;
    
    public BSGSTAB(LUPreconditioner luPreconditioner, LUSparse luSparse)
    {
        _luPreconditioner = luPreconditioner;
        _luSparse = luSparse;
    }
    public override Vector Solve(SparseMatrix globalMatrix, Vector globalVector)
    {
        solution = new Vector(globalVector.Size);
        PrepareProcess(globalMatrix, globalVector);
        return IterationProcess(globalMatrix, globalVector);
    }
    
    private void PrepareProcess(SparseMatrix globalMatrix, Vector globalVector)
    {
        _preconditionMatrix = _luPreconditioner.Decompose(globalMatrix);
        _r0 = _luSparse.CalcY(_preconditionMatrix, CustomMath.Subtract(globalVector, CustomMath.MultiplyMatrixByVector(globalMatrix, solution)));
        _z = _r0;
        _r = _r0;
    }
    
    private Vector IterationProcess(SparseMatrix globalMatrix, Vector globalVector)
    {
        Console.WriteLine("BCGSTAB");

        var residual = _r0.Lenght() / globalVector.Lenght();

        for (var i = 1; i <= SlaeParams.MaxIterations && residual > Math.Pow(SlaeParams.Eps, 2); i++)
        {
            var scalarRR = CustomMath.ScalarProduct(_r, _r0);

            var LAUz = _luSparse.CalcY(_preconditionMatrix, CustomMath.MultiplyMatrixByVector(globalMatrix, _luSparse.CalcX(_preconditionMatrix, _z)));

            var alpha = scalarRR / CustomMath.ScalarProduct(_r0, LAUz);

            var p = CustomMath.Subtract(_r, alpha * LAUz);

            var LAUp = _luSparse.CalcY(_preconditionMatrix, CustomMath.MultiplyMatrixByVector(globalMatrix, _luSparse.CalcX(_preconditionMatrix, p)));

            var gamma = CustomMath.ScalarProduct(p, LAUp) / CustomMath.ScalarProduct(LAUp, LAUp);

            CustomMath.Sum(solution, CustomMath.Sum(alpha *_z, gamma * p));

            var rNext = CustomMath.Subtract(p, gamma * LAUp);

            var beta = alpha * CustomMath.ScalarProduct(rNext, _r0) / (gamma * scalarRR);

            _z = CustomMath.Sum
            (
                CustomMath.Subtract(CustomMath.Multiply(beta, _r),
                    CustomMath.Multiply(beta * gamma, LAUz)),
                rNext
            );

            _r = rNext;

            residual = _r.Lenght() / globalVector.Lenght();
        }

        _luSparse.CalcXWithoutMemory(_preconditionMatrix, solution);

        Console.WriteLine();

        return solution;
    }
}