using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using IAsset.Controllers;
using IAsset.Models;
using IAsset.Models.DTOs;
using IAsset.Service_References.ServiceReferenceGlobalWeather;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TechTalk.SpecFlow;

namespace IAsset.Test
{
  [Binding]
  public class GetWeatherAppControllerTestSteps
  {
    private Mock<IClientAdapter<GlobalWeatherSoapClient>> _mainClientMock;
    private Mock<IClientAdapter<HttpClient>> _fallbackClientMock;
    private AppController _appController;
    private IHttpActionResult _result;

    [Given(@"we create an AppController")]
    public void GivenWeCreateAnAppController()
    {
      // Arrange
      _mainClientMock = new Mock<IClientAdapter<GlobalWeatherSoapClient>>();

      _fallbackClientMock = new Mock<IClientAdapter<HttpClient>>();
      _fallbackClientMock.Setup(x => x.GetWeatherAsync("Australia", "Sydney")).Returns(Task.FromResult<string>("\r\n{\"coord\":{\"lon\":123,\"lat\":456},\"weather\":[{\"id\":1,\"main\":\"Clear\",\"description\":\"clear sky\",\"icon\":\"01n\"}],\"base\":\"stations\",\"main\":{\"temp\":12,\"pressure\":9000,\"humidity\":9001,\"temp_min\":12,\"temp_max\":12},\"visibility\":9002,\"wind\":{\"speed\":2.1,\"deg\":230},\"clouds\":{\"all\":0},\"dt\":1499340600,\"sys\":{\"type\":1,\"id\":8233,\"message\":0.0098,\"country\":\"AU\",\"sunrise\":1499288429,\"sunset\":1499324377},\"id\":2147714,\"name\":\"Sydney\",\"cod\":200}"));
      _fallbackClientMock.Setup(x => x.GetWeatherAsync("Australia", "NotACity")).Throws(new HttpRequestException());

      _appController = new AppController(_mainClientMock.Object, _fallbackClientMock.Object);
    }

    [When(@"we call the service with the country ""(.*)"" and the city ""(.*)""")]
    public async void WhenWeCallTheServiceWithTheCountryAndTheCity(string p0, string p1)
    {
      _result = await _appController.GetWeather(p0, p1);
    }

    [Then(@"we get the weather of ""(.*)""")]
    public void ThenWeGetTheCity(string p0)
    {
      Assert.IsTrue(_result is OkNegotiatedContentResult<CityInfoSend>);
      var result = (OkNegotiatedContentResult<CityInfoSend>) _result;

      // Assert
      Assert.AreEqual(result.Content.name, "Sydney");
      Assert.AreEqual(result.Content.coord.lon, 123);
      Assert.AreEqual(result.Content.coord.lat, 456);
      // over 9000
      Assert.AreEqual(result.Content.humidity, 9001);
      Assert.AreEqual(result.Content.pressure, 9000);
      Assert.AreEqual(result.Content.skycondition, "Clear");
      Assert.AreEqual(result.Content.temp, 12);
      Assert.AreEqual(result.Content.wind.speed, 2.1);
      Assert.AreEqual(result.Content.visibility, 9002);
    }

    [Then(@"we get an empty response")]
    public void ThenWeGetEnEmptyResponse()
    {
      Assert.IsTrue(_result is OkNegotiatedContentResult<object>);
    }

    [Given(@"we create an AppController with Exception throwing client")]
    public void GivenWeCreateAnAppControllerWithExceptionThrowingClient()
    {
      _mainClientMock = new Mock<IClientAdapter<GlobalWeatherSoapClient>>();

      _fallbackClientMock = new Mock<IClientAdapter<HttpClient>>();
      _fallbackClientMock.Setup(x => x.GetWeatherAsync("Australia", "Sydney")).Throws(new Exception());

      _appController = new AppController(_mainClientMock.Object, _fallbackClientMock.Object);
    }

    [Then(@"we get an error")]
    public void ThenWeGetAnError()
    {
      Assert.IsTrue(_result is ExceptionResult);
    }
  }
}
