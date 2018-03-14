using System.Web;
using System.Web.Mvc;

namespace Infobasis.Api
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)//zhuce 全局过滤器集合 GlobalFilterCollection 对象
        {
            filters.Add(new HandleErrorAttribute());//默认情况 HandleErrorAttribute过滤器被添加到集合中
            //异常过滤器 注册为全局异常过滤器，从而捕捉项目中所有的异常
        }
    }
}