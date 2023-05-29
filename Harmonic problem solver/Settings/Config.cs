using Application.Core.DataTypes;
using Application.DataTypes.Time;
using MathLibrary.DataTypes;

namespace Application;

public static class Config
{
    public static AxisInfo XAxisInfo = new AxisInfo()
    {
        SplitsNum = 9,
        StartPoint = new Point(0.0),
        InitialStep = ((double)1 / 9),
        StepMultiplier = 1.0
    };
    
    public static AxisInfo YAxisInfo = new AxisInfo()
    {
        SplitsNum = 9,
        StartPoint = new Point(0.0),
        InitialStep = ((double)1 / 9),
        StepMultiplier = 1.0
    };
    
    public static AxisInfo ZAxisInfo = new AxisInfo()
    {
        SplitsNum = 9,
        StartPoint = new Point(0.0),
        InitialStep = ((double)1 / 9),
        StepMultiplier = 1.0
    };

    public static double Sigma = 1.0;
    public static double Lambda = 2.0;
    public static double Omega = 1.0;
    public static double Eps = 0.01;

    public static Func<Point, double, double> Fs = (x, t) => -1.01 * x[0] + 0.99 * x[1] + 0.99 * x[2];
    public static Func<Point, double, double> Fc = (x, t) => 0.99 * x[0] + 1.01 * x[1] + 1.01 * x[2];
}