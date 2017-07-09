using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Results;
using IAsset.Controllers;
using IAsset.Models;
using IAsset.Models.DTOs;
using IAsset.Service_References.ServiceReferenceGlobalWeather;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpleInjector;

namespace IAsset.Test
{
  [TestClass]
  public class GetCitiesByCountryAppControllerTest
  {
    private Container _container;
    private Mock<IClientAdapter<GlobalWeatherSoapClient>> _mainClientMock;
    private Mock<IClientAdapter<HttpClient>> _fallbackClientMock;

    [TestInitialize]
    public void TestMethod()
    {
      _container = new Container();

      _mainClientMock = new Mock<IClientAdapter<GlobalWeatherSoapClient>>();
      _mainClientMock.Setup(x => x.GetCitiesByCountryAsync(TestHelpers.NoData)).Returns(Task.FromResult<string>(TestHelpers.Cities[TestHelpers.NoData]));
      _mainClientMock.Setup(x => x.GetCitiesByCountryAsync(TestHelpers.OneCity)).Returns(Task.FromResult<string>(TestHelpers.Cities[TestHelpers.OneCity]));
      _mainClientMock.Setup(x => x.GetCitiesByCountryAsync(TestHelpers.ManyCities)).Returns(Task.FromResult<string>(TestHelpers.Cities[TestHelpers.ManyCities]));
      _mainClientMock.Setup(x => x.GetCitiesByCountryAsync(TestHelpers.Incorrect)).Returns(Task.FromResult<string>(TestHelpers.Cities[TestHelpers.Incorrect]));

      _fallbackClientMock = new Mock<IClientAdapter<HttpClient>>();

      _container.Register<IClientAdapter<GlobalWeatherSoapClient>>(() => _mainClientMock.Object);
      _container.Register<IClientAdapter<HttpClient>>(() => _fallbackClientMock.Object);
    }

    [TestMethod]
    public async Task GetCitiesnoData()
    {
      // Arrange
      var appController = DefaultAppController();

      // Act
      var result = await appController.GetCities(TestHelpers.NoData);

      // Assert
      Assert.IsTrue(result is ExceptionResult);
    }

    [TestMethod]
    public async Task GetCitiesIncorrectXml()
    {
      // Arrange
      var appController = DefaultAppController();

      // Act
      var result = await appController.GetCities(TestHelpers.Incorrect);

      // Assert
      Assert.IsTrue(result is ExceptionResult);
    }

    [TestMethod]
    public async Task GetCitiesOneCity()
    {
      // Arrange
      var appController = DefaultAppController();

      // Act
      var result = await appController.GetCities(TestHelpers.OneCity) as OkNegotiatedContentResult<IEnumerable<City>>;

      // Assert
      Assert.AreEqual(result?.Content.Count(), 1);
      Assert.AreEqual(result?.Content.FirstOrDefault()?.Item, "Germany / Berlin-Schoenefeld");
      Assert.AreEqual(result?.Content.FirstOrDefault()?.Value, "Berlin-Schoenefeld,DE");
    }

    [TestMethod]
    public async Task GetCitiesManyCities()
    {
      // Arrange
      var appController = DefaultAppController();

      // Act
      var result = await appController.GetCities(TestHelpers.ManyCities) as OkNegotiatedContentResult<IEnumerable<City>>;

      // Assert
      Assert.AreEqual(result?.Content.Count(), 5);
    }

    // C# 6: Expression-bodied function members
    public AppController DefaultAppController() => new AppController(_container.GetInstance<IClientAdapter<GlobalWeatherSoapClient>>(), _container.GetInstance<IClientAdapter<HttpClient>>());
  }
}
