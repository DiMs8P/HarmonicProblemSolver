using Application.Core.DataTypes;
using Application.DataTypes.Time;
using MathLibrary.DataTypes;

namespace Application;

public static class Config
{
    public static AxisInfo XAxisInfo = new AxisInfo()
    {
        SplitsNum = 1,
        StartPoint = new Point(0.0),
        InitialStep = ((double)1 / 1),
        StepMultiplier = 1.0
    };
    
    public static AxisInfo YAxisInfo = new AxisInfo()
    {
        SplitsNum = 1,
        StartPoint = new Point(0.0),
        InitialStep = ((double)2 / 1),
        StepMultiplier = 1.0
    };
    
    public static AxisInfo ZAxisInfo = new AxisInfo()
    {
        SplitsNum = 1,
        StartPoint = new Point(0.0),
        InitialStep = ((double)3 / 1),
        StepMultiplier = 1.0
    };

    /*public static TimeInfo TimeInfo = new TimeInfo()
    {
        StartTime = 0.0,
        TimesNum = 31,
        InitialStep = 3d / 30,
        StepMultiplier = 1.0
    };*/

    public static double Sigma = 1.0;
    public static double Lambda = 1.0;
    public static double Omega = 1.0;
    public static double Eps = 1.0;

    // public static Func<double, double> U0 = x => x;
    //
    // public static Func<double, double, double> U = (x, t) => x + t;

    public static Func<Point, double, double> Fs = (x, t) => x[0] + 1;
    public static Func<Point, double, double> Fc = (x, t) => x[0] + 1;



    public static double Relaxation = 1.5d;
}