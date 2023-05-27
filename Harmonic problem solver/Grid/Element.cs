using Application.Core.DataTypes.Calculus;
namespace Application.Core.DataTypes;

public class Element
{
    public int NumberOfIndexes => _indexes.Length;
    public ReadOnlySpan<int> Indexes => _indexes;
    
    private int[] _indexes;
    private BaseFunction[] _functions;

    public Element(int[] indexes, BaseFunction[] functions)
    {
        _indexes = indexes;
        _functions = functions;
    }
    
    public new int this[int index] => _indexes[index];
}