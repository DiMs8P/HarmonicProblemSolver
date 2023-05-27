using MathLibrary.DataTypes;
using MathLibrary.DataTypes.Internal;
using MathLibrary.Optimization;

namespace MathLibrary;

public class QuadraticInterpolation : IExtremumFinder
{
    private  double _initialStep;
    private readonly double _errorTolerance = 1e-3;

    public QuadraticInterpolation(double initialStep)
    {
        _initialStep = initialStep;
    }
    public Point FindMinimum(Func<Point, double> func, Interval interval, in double errorTolerance)
    {
        if (interval.LeftBorder.Size != interval.RightBorder.Size)
        {
            throw new ArgumentException("Points has different dimension");
        }
        
        double intervalLenght = (interval.RightBorder - interval.LeftBorder).Lenght();
        _initialStep = intervalLenght / 10;
        Vector direction = (interval.RightBorder - interval.LeftBorder).Normalize();

        double x2 = intervalLenght / 2;
        double x1 = x2 - _initialStep;
        double x3 = x2 + _initialStep;
        
         double[] funcValues = new double[3];

         while (true)
         {
             funcValues[0] = func(interval.LeftBorder + x1 * direction);
             funcValues[1] = func(interval.LeftBorder + x2 * direction);
             funcValues[2] = func(interval.LeftBorder + x3 * direction);

             double minimumPoint = CalcMinimum_Internal(x1, x2, x3, funcValues);

             if (ExitCondition(x2, minimumPoint, func, interval, direction))
             {
                 return interval.LeftBorder + (minimumPoint +x2)/2 * direction;
             }
             
             x2 = minimumPoint;
             x1 = minimumPoint - _initialStep;
             x3 = minimumPoint + _initialStep;
             _initialStep /= 2;
         }
    }

    private bool ExitCondition(double b, double d, Func<Point, double> func, Interval interval, Vector direction)
    {
        double toleranceX = b < _errorTolerance ? 0 : Double.Abs((d - b) / b);
        double toleranceF = Double.Abs((func(interval.LeftBorder + d * direction) - func(interval.LeftBorder + b * direction)) / func(interval.LeftBorder + b * direction));
        return ( toleranceX< _errorTolerance) && 
               (toleranceF < _errorTolerance);
    }
    
    private double CalcMinimum_Internal(double x1Value, double x2Value, double x3Value, double[] funcValues)
    {
        double top = (x2Value * x2Value - x3Value * x3Value) * funcValues[0] +
                     (x3Value * x3Value - x1Value * x1Value) * funcValues[1] +
                     (x1Value * x1Value - x2Value * x2Value) * funcValues[2];
        
        double bottom = (x2Value - x3Value) * funcValues[0] +
                        (x3Value - x1Value) * funcValues[1] +
                        (x1Value - x2Value) * funcValues[2];
    
        return 0.5 * top / bottom;
    }
}