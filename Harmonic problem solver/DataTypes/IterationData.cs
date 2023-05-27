using Application.Core.DataTypes;
using MathLibrary.DataTypes;

namespace Application.DataTypes;

public class IterationData
{
    public Element Element;
    public Vector Solution;
    public double TimeStep;
    public double Time;
    public double[] CoordStep;
}