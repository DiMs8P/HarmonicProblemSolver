using MathLibrary.DataTypes;

namespace Application.Core;

public readonly record struct BaseFunction(Func<Point, double> func);
