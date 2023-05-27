using MathLibrary.DataTypes;
using MathLibrary.DataTypes.Internal;

namespace MathLibrary.Optimization;

public class ExtremumHelper
{
    private readonly IIntervalFinder _intervalFinder;
    private readonly IExtremumFinder _extremumFinder;
    public ExtremumHelper(IExtremumFinder? extremumFinder = null, IIntervalFinder? intervalFinder = null)
    {
        _intervalFinder = intervalFinder ?? new IntervalSearch(0.1);
        _extremumFinder = extremumFinder ?? new Dichotomy();
    }
    public Point FindMinimumPoint(Func<Point, double> func, Point startPoint, Vector direction, double errorTolerance)
    {
        Interval interval = _intervalFinder.Find(func, startPoint, direction);
        return _extremumFinder.FindMinimum(func, interval, errorTolerance);
    }
}