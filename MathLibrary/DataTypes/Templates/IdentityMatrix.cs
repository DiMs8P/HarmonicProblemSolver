namespace MathLibrary.DataTypes.Templates;

public class IdentityMatrix : Matrix
{
    public IdentityMatrix(int size) :base(CreateIdentityMatrix(size))
    {
    }
    
    private static double[,] CreateIdentityMatrix(int size)
    {
        double[,] values = new double[size, size];
        for (int i = 0; i < size; i++)
        {
            values[i, i] = 1;
        }

        return values;
    }
}