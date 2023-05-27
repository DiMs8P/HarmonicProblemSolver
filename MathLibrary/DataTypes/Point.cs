using System.Collections;

namespace MathLibrary.DataTypes;

public class Point : IEnumerable<double>
{
    private double[] _values;
    public int Size => _values.Length;
    
    public Point(params double[] values) => _values = values;
    
    public IEnumerator<double> GetEnumerator()
    {
        return ((IEnumerable<double>)_values).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _values.GetEnumerator();
    }

    public double this[int index]
    {
        get => _values[index];
        set => _values[index] = value;
    }
    
    public override string ToString() => $"Point({string.Join(", ", _values)})";
    
    public Point(Point copyFromPoint) =>_values = copyFromPoint._values.Select(x => x).ToArray();

    // Operations with Point
    public static Vector operator -(Point a, Point b) =>
        new Vector(a._values.Select((value, index) => value - b._values[index]).ToArray());
    
    // Operations with Vector
    public static Point operator +(Point a, Vector b) =>
        new Point(a._values.Select((value, index) => value + b[index]).ToArray());
    
    // Operations with Matrix
    // Operations with double
    public static Point operator +(Point a, double b) =>
        new Point(a._values.Select((value, index) => value + b).ToArray());
    
}