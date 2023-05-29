using System;
using Application.Core;
using Application.Core.Conditions;
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
            
            var firstConditionsProvider =
                new FirstBoundaryConditionProvider
                (
                    grid,
                    p => p[0] + p[1] + p[2],
                    p => p[0] - p[1] - p[2]
                );
            
            var firstConditions =
                firstConditionsProvider
                    .GetConditions
                    (
                        Config.XAxisInfo.SplitsNum, Config.YAxisInfo.SplitsNum, Config.ZAxisInfo.SplitsNum
                    );
            
            var secondConditionsProvider =
                new SecondBoundaryConditionProvider(grid);
            
            var secondConditions =
                secondConditionsProvider
                    .CreateConditions
                    (
                        new[] { 0 },
                        new[] { Bound.Back },
                        p => 1,
                        p => -1
                    )
                    .CreateConditions
                    (
                        new[] { 0 },
                        new[] { Bound.Left },
                        p => -1,
                        p => -1
                    )
                    .CreateConditions
                    (
                        new[] { 0 },
                        new[] { Bound.Right },
                        p => 1,
                        p => 1
                    )
                    .CreateConditions
                    (
                        new[] { 0 },
                        new[] { Bound.Upper },
                        p => 1,
                        p => -1
                    )
                    .CreateConditions
                    (
                        new[] { 0 },
                        new[] { Bound.Lower },
                        p => -1,
                        p => 1
                    )
                    .GetConditions();

            Appyer applyer = new Appyer();
            //applyer.ApplySecondConditions(secondConditions, vector);
            applyer.ApplyFirstConditions(matrix, vector, firstConditions);
            
            LUPreconditioner preconditioner = new LUPreconditioner();
            SlaeSolver solver = new SlaeSolver(new BSGSTAB(preconditioner, new LUSparse(preconditioner)));
            Vector solution = solver.Solve(matrix, vector);

            foreach (var item in solution)
            {
                Console.WriteLine(item);
            }
        }
    }
}