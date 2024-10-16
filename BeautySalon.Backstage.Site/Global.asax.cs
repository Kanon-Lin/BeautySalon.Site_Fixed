using AutoMapper;
using BeautySalon.Backstage.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BeautySalon.Backstage.Site
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static IMapper _mapper;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //AutoMapper配置
            var config = new MapperConfiguration(cfg =>
            {
                //使用MappingProfile來設定對應關係
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = config.CreateMapper();
        }



        //protected void Application_BeginRequest()
        //{
        //    var context = HttpContext.Current;

        //    if (context != null && context.User != null &&
        //        !context.User.Identity.IsAuthenticated &&
        //        !context.Request.Path.StartsWith("/Users/Login") &&
        //        !context.Request.Path.StartsWith("/Content") && // 允許靜態資源訪問
        //        !context.Request.Path.StartsWith("/Scripts"))
        //    {
        //        // 若未登入，則重定向至登入頁面
        //        context.Response.Redirect("~/Users/Login");
        //    }
        //}

    }
}
