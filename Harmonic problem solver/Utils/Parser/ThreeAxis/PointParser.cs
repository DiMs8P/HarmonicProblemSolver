using Application.Core;
using Application.Core.DataTypes;
using MathLibrary.DataTypes;

namespace Application.Utils;

public class PointParser : IParser<Point>
{
    private AxisInfo[] _data;

    public PointParser(params AxisInfo[] data)
    {
        _data = data;
    }

    public Point[] Parse()
    {
        Point[] points = new Point[_data.Aggregate(1, (acc, axisInfo) => acc * (axisInfo.SplitsNum + 1))];

        double initialZStep = _data[2].InitialStep;
        double initialYStep = _data[1].InitialStep;
        double initialXStep = _data[0].InitialStep;
        double currentZPoint = _data[2].StartPoint[0];
        double currentYPoint = _data[1].StartPoint[0];
        double currentXPoint = _data[0].StartPoint[0];
        
        for (int i = 0; i < _data[2].SplitsNum + 1; i++)
        {
            for (int j = 0; j < _data[1].SplitsNum + 1; j++)
            {
                for (int k = 0; k < _data[0].SplitsNum + 1; k++)
                {
                    // points[(_data[2].SplitsNum + 1) * i + (_data[1].SplitsNum + 1) * j + k] = new Point(
                    //     currentXPoint + initialXStep * Double.Pow(_data[0].StepMultiplier, k -1),
                    //     currentYPoint + initialYStep * Double.Pow(_data[1].StepMultiplier, j - 1),
                    //     currentZPoint + initialZStep * Double.Pow(_data[2].StepMultiplier, i - 1)
                    //     );
                    // currentXPoint = currentXPoint + initialXStep * Double.Pow(_data[0].StepMultiplier, k);
                    // currentYPoint = currentYPoint + initialYStep * Double.Pow(_data[1].StepMultiplier, j);
                    // currentZPoint = currentZPoint + initialZStep * Double.Pow(_data[2].StepMultiplier, i);
                    
                    points[((_data[1].SplitsNum + 1) * (_data[0].SplitsNum + 1)) * i + (_data[0].SplitsNum + 1) * j + k] = new Point(
                        currentXPoint + initialXStep * k,
                        currentYPoint + initialYStep * j,
                        currentZPoint + initialZStep * i);

                }
            }
        }
        
        return points;
    }
}