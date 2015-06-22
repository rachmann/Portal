using System.Web;
using System.Web.Mvc;
using Portal.Filters;

namespace Portal
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());
            filters.Add(new ExceptionLoggingFilterAttribute());
            filters.Add(new RequireHttpsAttribute());
        }
    }
}
