using System.Web.Mvc;

namespace CarShareApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(
            GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}