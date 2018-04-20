using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using CustomFunctionsWebApi.Models;
using System.Web.Http.OData.Routing.Conventions;
using System.Web.Http.OData.Routing;

namespace CustomFunctionsWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.AddODataQueryFilter();

            /*
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            */
          
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder(config);
            builder.EntitySet<DatabaseName>("Databases");
            builder.EntitySet<CustomFunctionSummary>("FunctionSummaries");
            builder.EntitySet<CustomFunction>("Functions");
            builder.EntitySet<CustomFunctionAttribute>("Attributes");
            builder.EntitySet<MethodCall>("MethodCalls");
            builder.EntitySet<Method>("Methods");
            builder.EntitySet<MethodCallArgument>("MethodCallArguments");
            builder.EntitySet<Namespace>("Namespaces");
            builder.EntitySet<MethodCallSummary>("MethodCallSummaries");

            var conventions = ODataRoutingConventions.CreateDefault();
            conventions.Insert(0, new CustomFunctionsWebApi.RoutingConventions.CompositeKeyRoutingConvention());
 
            config.Routes.MapODataServiceRoute("odata", "api", builder.GetEdmModel(),new DefaultODataPathHandler(),conventions);
        }
    }
}
