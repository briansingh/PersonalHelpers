// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="">
//   Copyright © 2014 
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

[assembly: Microsoft.Owin.OwinStartup(typeof(App.CustomFunctionWebApp_001.Startup))]

namespace App.CustomFunctionWebApp_001
{
    using Owin;
    using System.Web.Http;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //// For more information on how to configure your application, visit:
            //// http://go.microsoft.com/fwlink/?LinkID=316888

            this.ConfigureAuth(app);

            // Configure Web API for self-hosting
            HttpConfiguration config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller/{id}",
                defaults: new { id = RouteParameter.Optional });

            app.UseWebApi(config);
        }
    }
}
