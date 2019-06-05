using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ex3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //no URL given
            routes.MapRoute(
                name: "Port",
                url: "{action}/{ip}",
                defaults: new { controller = "Flight", action = "Index", ip = UrlParameter.Optional }
            );

            //case: display/127.0.0.1/5400/4
            routes.MapRoute("display", "display/{ip}/{port}/{time}",
            defaults: new { controller = "Flight", action = "display", ip = UrlParameter.Optional, port = UrlParameter.Optional, time = 0 });

            //case: save/127.0.0.1/5400/10/4
            routes.MapRoute(
               name: "Save",
               url: "{action}/{ip}/{port}/{time}/{countDown}/{fileName}",
               defaults: new { controller = "Flight" }
           );

        }
    }
}