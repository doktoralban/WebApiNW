using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApiNW
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("api/Siparisler/GetSiparisDetaybyOrderID"
                , "api/Siparisler/GetSiparisDetaybyOrderID"
            , new
            {
                Controller = "Siparisler"
            ,
                Action = "GetSiparisDetaybyOrderID"
            }
            );

            config.Routes.MapHttpRoute("a1"
              , "a2"
          , new
          {
              Controller = "Siparisler"
          ,
              Action = "MiktaraGore"
          }
          );

            config.Routes.MapHttpRoute(
                name: "DefaultApi-12",
                routeTemplate: " {action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );




        }
    }
}
