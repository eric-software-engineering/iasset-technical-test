using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using IAsset.Models;
using IAsset.Service_References.ServiceReferenceGlobalWeather;
using SimpleInjector;
using SimpleInjector.Lifestyles;

using SimpleInjector.Integration.WebApi;

namespace IAsset
{
  public static class WebApiConfig
  {
    public static void Register(HttpConfiguration config)
    {
      // Web API configuration and services
      var container = new Container();
      container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

      // Registering types
      container.Register<IClientAdapter<GlobalWeatherSoapClient>, WebservicexClientAdapter>(Lifestyle.Scoped);
      container.Register<IClientAdapter<HttpClient>, OpenweathermapClientAdapter>(Lifestyle.Scoped);

      // This is an extension method from the integration package.
      container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

      container.Verify();

      GlobalConfiguration.Configuration.DependencyResolver =
        new SimpleInjectorWebApiDependencyResolver(container);

      // Web API routes
      config.MapHttpAttributeRoutes();

      config.Routes.MapHttpRoute(
          name: "DefaultApi",
          routeTemplate: "api/{controller}/{id}",
          defaults: new { id = RouteParameter.Optional }
      );

      config.Formatters.JsonFormatter.SupportedMediaTypes
        .Add(new MediaTypeHeaderValue("text/html"));
    }
  }
}
