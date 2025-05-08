using System.Net.Http.Json;

namespace BIPractice.Web.Pages.PieChart.CountOfTypes;

public class CountOfTypesHttpClient(ILogger<CountOfTypesHttpClient> logger, HttpClient client)
{
    public async Task<IEnumerable<ResponseDto>> GetData()
    {
        try
        {
            return await client
                .GetFromJsonAsync<IEnumerable<ResponseDto>>("http://localhost:5186/pie") ??
                   throw new Exception("ответ пришел пустым");
            
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex.Message);
            throw;
        }
    }
}