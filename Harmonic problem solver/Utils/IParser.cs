using Application.Core.DataTypes;

namespace Application.Utils;

public interface IParser<T>
{
    public T[] Parse();
}