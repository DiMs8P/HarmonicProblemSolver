using Application.Core.DataTypes;
using MathLibrary.DataTypes;

namespace Application.DataTypes;

public readonly record struct MethodData(
    Func<Point, double, double> Fs,
    Func<Point, double, double> Fc,
    double Lambda,
    double Sigma,
    double Omega,
    double Eps
    );