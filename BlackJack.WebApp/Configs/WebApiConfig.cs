using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json.Serialization;

namespace BlackJack.WebApp.Configs
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var corsAttribute = new EnableCorsAttribute("http://localhost:4200", "*", "*");
            config.EnableCors(corsAttribute);
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}