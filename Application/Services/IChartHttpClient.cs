using Core.Enums;

namespace Application.Services;

public interface IChartHttpClient<T>
{
    Task<IEnumerable<T>> GetData(TypesOfGraph typesOfGraph);
}