namespace Application.Core.DataTypes.Calculus.Implementations;

public class Function : IFunction
{
    public BaseFunction Get(BaseFunctionData data)
    {
        return new BaseFunction(value => data.X(value[0]) * data.Y(value[1])*data.Z(value[2]));
    }
}