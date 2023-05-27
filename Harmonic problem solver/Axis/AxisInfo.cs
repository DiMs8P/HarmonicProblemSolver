using MathLibrary.DataTypes;

namespace Application.Core.DataTypes;

public readonly record struct AxisInfo(Point StartPoint, int SplitsNum, double InitialStep, double StepMultiplier);