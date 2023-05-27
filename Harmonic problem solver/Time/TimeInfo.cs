namespace Application.DataTypes.Time;

public readonly record struct TimeInfo(double StartTime, int TimesNum, double InitialStep, double StepMultiplier);