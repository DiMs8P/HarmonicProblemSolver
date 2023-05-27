using MathLibrary.DataTypes;
using MathLibrary.DataTypes.Internal;
using MathLibrary.Optimization;

namespace MathLibrary;

public class Dichotomy : IExtremumFinder
{
    public Point FindMinimum(Func<Point, double> func, Interval interval, in double errorTolerance = 1e-04)
    {
        if (interval.LeftBorder.Size != interval.RightBorder.Size)
        {
            throw new ArgumentException("Points has different dimension");
        }

        double intervalLenght = (interval.RightBorder - interval.LeftBorder).Lenght();
        Vector direction = (interval.RightBorder - interval.LeftBorder).Normalize();

        int stepsNum = Convert.ToInt32(
            Math.Ceiling(
                Math.Log(intervalLenght / errorTolerance) / Math.Log(2)
            ));

        double delta = errorTolerance / 2;
        Point x1;
        Point x2;
        Point leftBound = interval.LeftBorder;
        Point rightBound = interval.RightBorder;
        for (int i = 0; i < stepsNum && (rightBound - leftBound).Lenght() > errorTolerance; i++)
        {
            x1 = leftBound + (((rightBound - leftBound).Lenght() - delta) / 2) * direction;  //(b + a - delta) / 2
            x2 = leftBound + (((rightBound - leftBound).Lenght() + delta) / 2) * direction;  //(b + a + delta) / 2;

            if (func(x1) > func(x2))
            {
                leftBound = x1;
            }
            else if (func(x1) < func(x2))
            {
                rightBound = x2;
            }
            else
            {
                leftBound = x1;
                rightBound = x2;
            }
        }

        for (int i = 0; i < leftBound.Size; i++)
        {
            leftBound[i] = (leftBound[i] + rightBound[i]) / 2;
        }

        return leftBound;
    }
}