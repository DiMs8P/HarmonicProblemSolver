using System.Collections;

namespace MathLibrary.DataTypes;

public class Vector : IEnumerable<double>
{
    private readonly double[] _values;
    public int Size => _values.Length;
    
    public Vector(params double[] values) => _values = values;
    
    public Vector(int size) => _values = new double[size];

    public Vector(double value,int size) => _values = new double[size].Select(x=>value).ToArray();

    public Vector(Vector copyFromVector) => _values = copyFromVector._values.Select(x => x).ToArray();
    
    public Vector Normalize()
    {
        double lenght = Lenght();
        return new Vector(_values.Select(value => value / lenght).ToArray());
    }

    public void Clear()
    {
        for (int i = 0; i < _values.Length; i++)
        {
            _values[i] = 0;
        }
    }
    
    public double Lenght() =>Math.Sqrt(_values.Aggregate(0.0, (acc, vectorValue) => acc + vectorValue * vectorValue));

    // Operations with Point
    // Operations with Vector
    public static Vector operator +(Vector a, Vector b) =>
        new Vector(a._values.Select((value, index) => value + b[index]).ToArray());
    
    public static Vector operator -(Vector a, Vector b) =>
        new Vector(a._values.Select((value, index) => value - b[index]).ToArray());
    
    // Operations with Matrix
    // Operations with double
    public static Vector operator +(Vector a, double b) =>
        new Vector(a._values.Select(value => value + b).ToArray());

    public static Vector operator *(double b, Vector a) =>
        new Vector(a._values.Select(value => value * b).ToArray());

    public double this[int index]
    {
        get => _values[index];
        set => _values[index] = value;
    }

    public IEnumerator<double> GetEnumerator()
    {
        return ((IEnumerable<double>)_values).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _values.GetEnumerator();
    }
}