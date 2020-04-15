using SigaClasses.Utils;
using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SigaClasses
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            LoadConfig();
        }
        private void LoadConfig()
        {
            string login      = ConfigurationManager.AppSettings["login"];
            string password   = ConfigurationManager.AppSettings["password"];
            string host       = ConfigurationManager.AppSettings["host"];
            string database   = ConfigurationManager.AppSettings["database"];
            string collection = ConfigurationManager.AppSettings["collection"];

            MongoHandler.StartOnMongo(login, password, host, database, collection);
            MongoHandler.ConnectToMongo();
        }
    }
}
