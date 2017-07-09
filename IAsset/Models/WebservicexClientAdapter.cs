using System;
using System.Threading.Tasks;
using IAsset.Service_References.ServiceReferenceGlobalWeather;

namespace IAsset.Models
{
  // SOLID Principle: [S]ingle Responsibility
  public class WebservicexClientAdapter : IClientAdapter<GlobalWeatherSoapClient>
  {
    // Adapter Design Pattern
    private readonly GlobalWeatherSoapClient _client = new GlobalWeatherSoapClient();

    public async Task<string> GetCitiesByCountryAsync(string country)
    {
      var response = await _client.GetCitiesByCountryAsync(new GetCitiesByCountryRequest(country));
      return response.GetCitiesByCountryResult;
    }

    public async Task<string> GetWeatherAsync(string country, string city)
    {
      var response = await _client.GetWeatherAsync(new GetWeatherRequest(country, city));
      return response.GetWeatherResult == "Data Not Found" ? null : response.GetWeatherResult;
    }

    public void Dispose()
    {
      _client.Close();
    }
  }
}