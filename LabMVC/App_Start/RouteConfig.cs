using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;

namespace LabMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
            name: "Login",
            url: "Login/Index",
            defaults: new { controller = "Login", action = "Index" }
        );


            routes.MapRoute(
            name: "cliente_salvar",
            url: "Clientes/Salvar",
            defaults: new { controller = "Clientes", action = "Salvar" }
            );

            routes.MapRoute(
            name: "Editar",
            url: "Home/Editar/{id}",
            defaults: new { controller = "Home", action = "Editar" }
            );

            routes.MapRoute(
            name: "BemVindo",
            url: "Home/BemVindo",
            defaults: new { controller = "Home", action = "BemVindo" }
            );

            routes.MapRoute(
            name: "Default",
             url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
