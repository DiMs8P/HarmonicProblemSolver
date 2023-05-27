using MathLibrary.DataTypes;
using MathLibrary.DataTypes.Internal;

namespace MathLibrary.Optimization;

public interface IExtremumFinder
{
    public abstract Point FindMinimum(Func<Point, double> func, Interval interval, in double errorTolerance = 1e-04);
}