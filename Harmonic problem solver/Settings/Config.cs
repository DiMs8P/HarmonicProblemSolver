using Application.Core.DataTypes;
using Application.DataTypes.Time;
using MathLibrary.DataTypes;

namespace Application;

public static class Config
{
    private static int XSplits = 4;
    private static int YSplits = 4;
    private static int ZSplits = 4;
    
    public static AxisInfo XAxisInfo = new AxisInfo()
    {
        SplitsNum = XSplits,
        StartPoint = new Point(0.0),
        InitialStep = ((double)1 / XSplits),
        StepMultiplier = 1.0
    };
    
    public static AxisInfo YAxisInfo = new AxisInfo()
    {
        SplitsNum = YSplits,
        StartPoint = new Point(0.0),
        InitialStep = ((double)1 / YSplits),
        StepMultiplier = 1.0
    };
    
    public static AxisInfo ZAxisInfo = new AxisInfo()
    {
        SplitsNum = ZSplits,
        StartPoint = new Point(0.0),
        InitialStep = ((double)1 / ZSplits),
        StepMultiplier = 1.0
    };

    public static double Sigma = 1.0;
    public static double Lambda = 5.0;
    public static double Omega = 2.0;
    public static double Eps = 0.01;

    public static Func<Point, double, double> Fs = (x, t) => -14 * Math.Exp(-x[0] - x[1]) - 2 * Math.Exp(-x[1] - x[2]);
    public static Func<Point, double, double> Fc = (x, t) => -14 * Math.Exp(-x[1] - x[2]) + 2 * Math.Exp(-x[0] - x[1]);
    /*    public static Func<Point, double, double> Fs = (x, t) => Math.Exp(-x[0] - x[1]);
        public static Func<Point, double, double> Fc = (x, t) => Math.Exp(-x[0] - x[1]);*/

    public static Func<Point, double> Us = (x) => Math.Exp(-x[0] - x[1]);
    public static Func<Point, double> Uc = (x) => Math.Exp(-x[1] - x[2]);
}