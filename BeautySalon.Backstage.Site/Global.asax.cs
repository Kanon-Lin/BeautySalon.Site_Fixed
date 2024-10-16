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

            //AutoMapper�t�m
            var config = new MapperConfiguration(cfg =>
            {
                //�ϥ�MappingProfile�ӳ]�w�������Y
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
        //        !context.Request.Path.StartsWith("/Content") && // ���\�R�A�귽�X��
        //        !context.Request.Path.StartsWith("/Scripts"))
        //    {
        //        // �Y���n�J�A�h���w�V�ܵn�J����
        //        context.Response.Redirect("~/Users/Login");
        //    }
        //}

    }
}
