using MathLibrary.DataTypes;

namespace Application.Core.Conditions.Conditions;

public readonly record struct ThirdCondition(Matrix Matrix, Vector Vector, int[] Indexes);