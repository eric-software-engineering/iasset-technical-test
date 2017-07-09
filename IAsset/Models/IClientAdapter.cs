using System;
using System.Threading.Tasks;

namespace IAsset.Models
{
  // SOLID Principle: [I]nterface Segregation
  public interface IClientAdapter<T> : IDisposable
  {
    Task<string> GetCitiesByCountryAsync(string country);
    Task<string> GetWeatherAsync(string country, string city);
  }
}