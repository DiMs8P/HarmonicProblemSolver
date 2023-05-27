using Application.Core;
using Application.Core.DataTypes;
using Application.Core.DataTypes.Calculus.Implementations;
using MathLibrary.DataTypes;

namespace Application.Utils.Parser.ThreeAxis;

public class ThreeAxisElementParser : IParser<Element>
{
    private AxisInfo[] _data;
    public ThreeAxisElementParser(params AxisInfo[] data)
    {
        _data = data;
    }

    public Element[] Parse()
    {
        PointContainer points = PointContainer.GetInstance();
        Element[] elems = new Element[_data[2].SplitsNum * _data[1].SplitsNum * _data[0].SplitsNum];
        
        for (int i = 0; i < _data[2].SplitsNum; i++)
        {
            for (int j = 0; j < _data[1].SplitsNum; j++)
            {
                for (int k = 0; k < _data[0].SplitsNum; k++)
                {
                    int[] indexes = GetIndexes(i, j, k);
                    List<List<Func<double, double>>> functionsPool = InitializePool(indexes, points);
                    
                    BaseFunction[] baseFunctions = GetBaseFunctionsFromPool(functionsPool);

                    int[] extendedIndexes = new int[indexes.Length * 2];
                    for (int q = 0; q < indexes.Length; q++)
                    {
                        extendedIndexes[2*q] = 2 * indexes[q];
                        extendedIndexes[2*q + 1] = 2 * indexes[q] + 1;
                    }
                    elems[((_data[1].SplitsNum + 1) * (_data[0].SplitsNum + 1)) * i + (_data[0].SplitsNum + 1) * j + k] = new Element(extendedIndexes, baseFunctions);
                }
            }
        }

        return elems;
    }

    private BaseFunction[] GetBaseFunctionsFromPool(List<List<Func<double, double>>> functionsPool)
    {
        List<BaseFunction> functions = new List<BaseFunction>();
        for (int i = 1; i <= 8; i++)
        {
             BaseFunctionData baseFunctionData = new BaseFunctionData(functionsPool[0][Eta(i) - 1], functionsPool[1][Nu(i) - 1], functionsPool[2][Third(i) - 1]);
             functions.Add(new Function().Get(baseFunctionData));
        }

        return functions.ToArray();
    }

    private List<List<Func<double, double>>> InitializePool(int[] indexes, PointContainer points)
    {
        List<List<Func<double, double>>> functionPool = new List<List<Func<double, double>>>(){new List<Func<double, double>>(), new List<Func<double, double>>(), new List<Func<double, double>>()};

        double xStep = points[indexes[1]][0] - points[indexes[0]][0];
        functionPool[0].Add(value => (points[indexes[1]][0] - value) / xStep);
        functionPool[0].Add(value => (value - points[indexes[0]][0]) / xStep);
        
        double yStep = points[indexes[2]][1] - points[indexes[0]][1];
        functionPool[1].Add(value => (points[indexes[2]][1] - value) / yStep);
        functionPool[1].Add(value => (value - points[indexes[0]][1]) / yStep);
        
        double zStep = points[indexes[4]][2] - points[indexes[0]][2];
        functionPool[2].Add(value => (points[indexes[4]][2] - value) / zStep);
        functionPool[2].Add(value => (value - points[indexes[0]][2]) / zStep);
        
        return functionPool;
    }

    private int[] GetIndexes(int i, int j, int k)
    {
        List<int> indexes = new List<int>();
        for (int q = 0; q < 2; q++)
        {
            for (int p = 0; p < 2; p++)
            {
                for (int f = 0; f < 2; f++)
                {
                    indexes.Add((_data[0].SplitsNum + 1)*(_data[1].SplitsNum + 1) * (i+ q) + (_data[0].SplitsNum + 1)*(j+ p) + (k + f));
                }
            }
        }

        return indexes.ToArray();
    }

    int Eta(int i) => ((i - 1) % 2) + 1;
    int Nu(int i) => ((int)((i - (double)1) / 2) % 2) + 1;
    int Third(int i) => (int)((i - (double)1) / 4) + 1;
}