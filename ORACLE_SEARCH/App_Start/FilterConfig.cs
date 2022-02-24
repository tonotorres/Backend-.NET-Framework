using System.Web;
using System.Web.Mvc;

namespace EPICX_ORACLE_SEARCH
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
