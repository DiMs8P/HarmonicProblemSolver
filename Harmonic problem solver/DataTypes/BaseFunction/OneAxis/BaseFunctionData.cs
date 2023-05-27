using MathLibrary.DataTypes;

namespace Application.Core.DataTypes;

public readonly record struct BaseFunctionData(Func<double, double> X, Func<double, double> Y, Func<double, double> Z);