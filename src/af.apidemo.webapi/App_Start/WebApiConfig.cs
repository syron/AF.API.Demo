using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace af.apidemo.webapi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //formatter.PluralizationService = new JSONAPI.Core.PluralizationService();
            JSONAPI.Json.JsonApiFormatter formatter = new JSONAPI.Json.JsonApiFormatter(new JSONAPI.Core.PluralizationService());
            GlobalConfiguration.Configuration.Formatters.Add(formatter);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
