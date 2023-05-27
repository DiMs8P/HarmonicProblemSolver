using MathLibrary.DataTypes;
using MathLibrary.DataTypes.Internal;

namespace MathLibrary.Optimization;

public interface IIntervalFinder
{
    public abstract Interval Find(Func<Point, double> func, Point startPoint, Vector direction);
}