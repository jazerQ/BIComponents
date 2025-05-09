using System.Net.Http.Json;
using Application.PieChart.GetCountOfAnyTypes;
using Core.Enums;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class ChartHttpClient<T>(ILogger<ChartHttpClient<T>> logger, HttpClient client) : IChartHttpClient<T>
{
    public async Task<IEnumerable<T>> GetData(TypesOfGraph typesOfGraph)
    {
        try
        {
            return await client
                       .GetFromJsonAsync<IEnumerable<T>>($"http://localhost:5186/{typesOfGraph.ToString().ToLower()}") ??
                   throw new Exception("ответ пришел пустым");
            
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex.Message);
            throw;
        }
    }
}