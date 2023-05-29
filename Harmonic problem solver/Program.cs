using System;
using Application.Core;
using Application.Core.DataTypes;
using Application.Core.DataTypes.Matrix;
using Application.Core.Global;
using Application.Core.Methods;
using Application.Core.Methods.Implementations;
using Application.Core.Methods.Implementations.BSGSTAB;
using Application.Core.Methods.Implementations.LOS;
using Application.DataTypes;
using Application.Utils;
using Application.Utils.Parser;
using Application.Utils.Parser.ThreeAxis;
using Iterative_methods.DataTypes.Matrix;
using Iterative_methods.DataTypes.Matrix.Implementations;
using MathLibrary.DataTypes;

namespace Application
{
    class Program
    {
        static void Main(string[] args)
        {
            IParser<Point> pointParser = new PointParser(Config.XAxisInfo, Config.YAxisInfo, Config.ZAxisInfo);
            IParser<Element> elementParser = new ThreeAxisElementParser(Config.XAxisInfo, Config.YAxisInfo, Config.ZAxisInfo);

            PointContainer.GetInstance().Initialize(pointParser);

            Grid grid = new Grid(elementParser);
            
            MethodData methodData = new MethodData()
            {
                Fs = Config.Fs,
                Fc = Config.Fc,
                Lambda = Config.Lambda,
                Sigma = Config.Sigma,
                Omega = Config.Omega,
                Eps = Config.Eps
            };
            IterationData iterationData = new IterationData()
            {
                Time = 0,
                CoordStep = new double[3],
            };

            GlobalMatrixFiller matrixFiller = new GlobalMatrixFiller(methodData);
            SparseMatrix matrix = new SparseMatrix(grid);
            matrixFiller.Fill(matrix, grid, iterationData);
            
            GlobalVectorFiller vectorFiller = new GlobalVectorFiller(methodData);
            Vector vector = new Vector(new Vector(PointContainer.GetInstance().Size * 2));
            vectorFiller.Fill(vector, grid, iterationData);

            
            // Triangle lowerTrianlge = new LowerTriangle(
            //         new int[]{0,1,3,4,6},
            //         new int[]{0, 0, 1, 1, 0, 3},
            //         new double[]{2, 5, 4, 1, 3, 1}
            //     );
            // double[] diag = new double[] { 6, 6, 6, 6, 6 };
            // Triangle upperTrianlge = new UpperTriangle(
            //     new int[]{3,5,5,6,6},
            //     new int[]{1,2,4,2,3,4},
            //     new double[]{3,3,5,5,4,5}
            //     );
            // SparseMatrix matrix = new SparseMatrix(lowerTrianlge, diag, upperTrianlge);
            //
            // Vector vector = new Vector(new double[] {5,2,4,1,6});

            LUPreconditioner preconditioner = new LUPreconditioner();
            SlaeSolver solver = new SlaeSolver(new LOS(preconditioner, new LUSparse(preconditioner)));
            Vector solution = solver.Solve(matrix, vector);

            foreach (var item in solution)
            {
                Console.WriteLine(item);
            }
        }
    }
}