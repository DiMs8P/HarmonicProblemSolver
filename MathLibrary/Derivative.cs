using MathLibrary.DataTypes;

namespace MathLibrary;

public static class Derivative
{
    public static double Calc(Func<Point, double> func, Point point, int order, int variableIndex, double derivativeTolerance)
    {
        if (order == 1)
        {
            return Calc_Internal(func, point, variableIndex, derivativeTolerance);
        }

        Point leftPoint = new Point(point);
        leftPoint[variableIndex] -= derivativeTolerance;
        double valueLeft = Calc(func, leftPoint, order - 1, variableIndex, derivativeTolerance);

        Point rightPoint = new Point(point);
        rightPoint[variableIndex] += derivativeTolerance;
        double valueRight = Calc(func, rightPoint, order - 1, variableIndex, derivativeTolerance);

        return (valueRight - valueLeft) / (2 * derivativeTolerance);
    }

    private static double Calc_Internal(Func<Point, double> func, Point point, int variableIndex, double derivativeTolerance)
    {
        Point leftPoint = new Point(point);
        leftPoint[variableIndex] -= derivativeTolerance;

        Point rightPoint = new Point(point);
        rightPoint[variableIndex] += derivativeTolerance;

        return (func(rightPoint) - func(leftPoint)) / (2 * derivativeTolerance);
    }

    public static double Divergence(Func<Point, double> func, Point point, double derivativeTolerance)
    {
        return Gradient(func, point, derivativeTolerance).Lenght();
    }

    public static Vector Gradient(Func<Point, double> func, Point point, double derivativeTolerance)
    {
        Vector output = new Vector(point.Size);

        for (int i = 0; i < point.Size; i++)
        {
            output[i] = Calc_Internal(func, point, i, derivativeTolerance);
        }

        return output;
    }
}