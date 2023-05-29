using Application.Core.Conditions.Conditions;
using Iterative_methods.DataTypes.Matrix;
using MathLibrary.DataTypes;

namespace Application.Core.Conditions;

public class Appyer
{
    private readonly Inserter _inserter;
    private readonly GaussExcluder _gaussExcluder;

    public Appyer()
    {
        _inserter = new Inserter();
        _gaussExcluder = new GaussExcluder();
    }
    
    public void ApplySecondConditions(SecondCondition[] conditions, Vector globalVector)
    {
        foreach (var condition in conditions)
        {
            _inserter.InsertVector(globalVector, condition.Vector, condition.Indexes);
        }
    }

    public void ApplyFirstConditions(SparseMatrix globalMatrix, Vector globalVector, FirstCondition[] conditions)
    {
        foreach (var condition in conditions)
        {
            _gaussExcluder.Exclude(globalMatrix, globalVector, condition);
        }
    }
}