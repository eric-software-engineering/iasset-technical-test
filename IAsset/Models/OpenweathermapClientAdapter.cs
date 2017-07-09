using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IAsset.Models
{
  // SOLID Principle: [S]ingle Responsibility
  public class OpenweathermapClientAdapter : IClientAdapter<HttpClient>
  {
    // Adapter Design Pattern
    private readonly HttpClient _httpClient = new HttpClient();

    public Task<string> GetCitiesByCountryAsync(string country)
    {
      throw new NotImplementedException();
    }

    public async Task<string> GetWeatherAsync(string country, string city)
    {
      // C# 6: String interpolation
      var url = $"http://api.openweathermap.org/data/2.5/weather?q={country},{city}&units=metric&appid=179c04c3390fc61f7b3d9a8a6a920729";
      var response = await _httpClient.GetAsync(url);

      // Service throws a 404 if the city if not found
      response.EnsureSuccessStatusCode();
      var responseBody = await response.Content.ReadAsStringAsync();
      return responseBody;
    }

    public void Dispose()
    {
      _httpClient.Dispose();
    }
  }
}