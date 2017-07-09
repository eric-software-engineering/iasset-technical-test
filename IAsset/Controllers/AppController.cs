using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;
using IAsset.Models;
using IAsset.Models.DTOs;
using IAsset.Service_References.ServiceReferenceGlobalWeather;

namespace IAsset.Controllers
{
  public class AppController : ApiController
  {
    private readonly IClientAdapter<GlobalWeatherSoapClient> _mainClientAdapter;
    private readonly IClientAdapter<HttpClient> _fallbackClientAdapter;

    // SOLID PRINCIPLE: [D]ependency Inversion
    public AppController(IClientAdapter<GlobalWeatherSoapClient> mainClientAdapter, IClientAdapter<HttpClient> fallbackClientAdapter)
    {
      _mainClientAdapter = mainClientAdapter;
      _fallbackClientAdapter = fallbackClientAdapter;
    }

    [Route("api/getcities/{country}")]
    public async Task<IHttpActionResult> GetCities(string country)
    {
      try
      {
        var xml = await _mainClientAdapter.GetCitiesByCountryAsync(country);
        var doc = new XmlDocument();
        doc.LoadXml(xml);

        // XPath
        var nodes = doc.SelectNodes("//Table");

        // C# 6: Null-conditional operator
        if (nodes?.Count == 0) return Ok(new { });

        var cities = nodes?.Cast<XmlNode>()
          .Select(node => new Tuple<string, string>(
            node.SelectSingleNode("Country")?.InnerText,
            node.SelectSingleNode("City")?.InnerText
          ));

        // The second service works better with the 2 letters region code. We add it
        var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.Name));

        // Equivalent of a SQL %LIKE% written in LINQ
        var result = (from city in cities
                      from region in regions
                      where region.EnglishName.ToLower().Contains(city.Item1.ToLower()) || city.Item1.ToLower().Contains(region.EnglishName.ToLower())
                      // Anonymous type
                      select new
                      {
                        // String interpolation
                        Item = $"{city.Item1} / {city.Item2}",
                        Value = Regex.Replace(city.Item2, @"[^a-zA-Z0-9 -]", string.Empty) + "," + region.TwoLetterISORegionName
                      })
                      .Distinct()
                      .Select(x => new City { Item = x.Item, Value = x.Value });

        return Ok(result);
      }
      catch (Exception e)
      {
        return InternalServerError(e);
      }
    }

    [Route("api/getweather/{country}/{city}")]
    public async Task<IHttpActionResult> GetWeather(string country, string city)
    {
      try
      {
        var citiesByCountry = await _mainClientAdapter.GetWeatherAsync(country, city);

        if (citiesByCountry != null)
        {
          // This service never returned any data for some reasons
          // I don't know what is the data structure ¯\_(ツ)_/¯
          throw new NotImplementedException();
        }
        // else keyword unnecessary here
        {
          var value = await _fallbackClientAdapter.GetWeatherAsync(country, city);
          var data = Newtonsoft.Json.JsonConvert.DeserializeObject<CityInfo>(value);

          return Ok(new CityInfoSend
          {
            name = data.name,
            coord = data.coord,
            wind = data.wind,
            visibility = data.visibility,
            skycondition = data.weather.FirstOrDefault()?.main,
            temp = data.main.temp,
            humidity = data.main.humidity,
            pressure = data.main.pressure
          });
        }
      }
      catch (HttpRequestException)
      {
        // No city found, return an empty JSON
        return Ok(new object());
      }
      catch (Exception e)
      {
        return InternalServerError(e);
      }
    }
  }
}
