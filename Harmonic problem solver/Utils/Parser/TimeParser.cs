using Application.DataTypes.Time;

namespace Application.Utils.Parser;

public class TimeParser : IParser<double>
{
    private TimeInfo _timeInfo;
    public TimeParser(TimeInfo timeInfo)
    {
        _timeInfo = timeInfo;
    }
    public double[] Parse()
    {
        double[] times = new double[_timeInfo.TimesNum];
        
        times[0] = _timeInfo.StartTime;
        for (int i = 1; i < times.Length; i++)
        {
            times[i] = times[i-1] + _timeInfo.InitialStep * Double.Pow(_timeInfo.StepMultiplier, i - 1);
        }

        return times;
    }
}