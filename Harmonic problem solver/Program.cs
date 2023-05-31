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
using Application.Core.Methods.Utils;
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
                    p => Math.Exp(-p[0] - p[1]),
                    p => Math.Exp(-p[1] - p[2])
                );
            
            var firstConditions =
                firstConditionsProvider
                    .GetConditions
                    (
                        Config.XAxisInfo.SplitsNum, Config.YAxisInfo.SplitsNum, Config.ZAxisInfo.SplitsNum
                    );
        
            Appyer applyer = new Appyer();
            //applyer.ApplySecondConditions(secondConditions, vector);
            applyer.ApplyFirstConditions(matrix, vector, firstConditions);





            Func<Point, double>[] Uuuu = new Func<Point, double>[]
            {
                    Config.Us,
                    Config.Uc
            };

            PointContainer Points = PointContainer.GetInstance();
            Vector naturalResult = new Vector(Points.Size*2);

            for (int i = 0; i < Points.Size; i++)
            {
                naturalResult[i*2] = Uuuu[0](Points[i]);
                naturalResult[i*2+1] = Uuuu[1](Points[i]);
            }



            DateTime start1 = DateTime.Now;

            LUPreconditioner preconditioner1 = new LUPreconditioner();
            SlaeSolver solver1 = new SlaeSolver(new BSGSTAB(preconditioner1, new LUSparse(preconditioner1)));
            Vector solution1 = solver1.Solve(matrix, vector);

            var norm1 = (solution1 - naturalResult).Lenght() / naturalResult.Lenght();
            Console.WriteLine("norm: {0}", norm1);

            /*            Console.WriteLine("bsgtttttt");
                        foreach (var item in solution1)
                        {
                            Console.WriteLine(item);
                        }*/

            DateTime end1 = DateTime.Now;
            TimeSpan ts1 = (end1 - start1);
            Console.WriteLine("1Elapsed Time is {0} ms", ts1.TotalMilliseconds);
            Console.WriteLine();
            DateTime start2 = DateTime.Now;

            LUPreconditioner preconditioner2 = new LUPreconditioner();
            SlaeSolver solver2 = new SlaeSolver(new LOS(preconditioner2, new LUSparse(preconditioner2)));
            Vector solution2 = solver2.Solve(matrix, vector);

            var norm2 = (solution2 - naturalResult).Lenght() / naturalResult.Lenght();
            Console.WriteLine("norm: {0}", norm2);

            /*            Console.WriteLine("losxdfsdfsdfsdf");
                        foreach (var item in solution2)
                        {
                            Console.WriteLine(item);
                        }*/

            DateTime end2 = DateTime.Now;
            TimeSpan ts2 = (end2 - start2);
            Console.WriteLine("2Elapsed Time is {0} ms", ts2.TotalMilliseconds);
            Console.WriteLine();








            DateTime start3 = DateTime.Now;

            var profileMatrix = MatrixConverter.Convert(matrix);

            LUPreconditioner preconditioner3 = new LUPreconditioner();
            LUProfile solver3 = new LUProfile();

            Vector solution3 = new Vector(1,solution2.Size);
            solution3 = solver3.Solve(profileMatrix, solution3, vector);


            Console.WriteLine("LU");

            var norm3 = (solution3 - naturalResult).Lenght() / naturalResult.Lenght();
            Console.WriteLine("norm: {0}", norm3);


            DateTime end3 = DateTime.Now;
            TimeSpan ts3 = (end3 - start3);
            Console.WriteLine("3Elapsed Time is {0} ms", ts3.TotalMilliseconds);

/*            Console.WriteLine("lufdgdfgdfgdg");
            foreach (var item in solution3)
            {
                Console.WriteLine(item);
            }*/
        }
    }
}