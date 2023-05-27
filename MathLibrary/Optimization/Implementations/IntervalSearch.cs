using MathLibrary;
using MathLibrary.DataTypes;
using MathLibrary.DataTypes.Internal;
using MathLibrary.Optimization;


public class IntervalSearch : IIntervalFinder
{
    private readonly double _initialStep;
    public IntervalSearch(double initialStep)
    {
        _initialStep = initialStep;
    }
    public Interval Find(Func<Point, double> func, Point startPoint, Vector direction)
    {
        //Vector normalizedDirection = direction.Normalize();
        Point prevPoint = new Point(startPoint);
        Point x1 = startPoint + _initialStep * direction;
        double h = _initialStep;

        while (func(x1) > func(x1 + h * direction))
        {
            prevPoint = x1;
            x1 += h * direction;
            h *= 2;
        }
        
        return new Interval(prevPoint, x1 + h * direction);
    } 
}