using System.Net.Http.Json;
using Application.PieChart.GetCountOfAnyTypes;
using Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class ChartHttpClient<T>(ILogger<ChartHttpClient<T>> logger, HttpClient client, IConfiguration config) : IChartHttpClient<T>
{
    public async Task<IEnumerable<T>> GetData(TypesOfGraph typesOfGraph)
    {
        try
        {
            #if DEBUG
            return await client
                       .GetFromJsonAsync<
                           IEnumerable<T>>($"http://localhost:5186/{typesOfGraph.ToString().ToLower()}") ??
                   throw new Exception("ответ пришел пустым");
            #endif
            
            #if RELEASE
            return await client
                       .GetFromJsonAsync<
                           IEnumerable<T>>($"{config["ServerUrl"]}/{typesOfGraph.ToString().ToLower()}") ??
                   throw new Exception("ответ пришел пустым");
            #endif  
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex.Message);
            return [];
        }
    }
}