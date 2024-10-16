using AutoMapper;
using BeautySalon.FrontEnd.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity.AspNet.WebApi;
using Unity;
using BeautySalon.FrontEnd.Site.Models.Interfaces;
using BeautySalon.FrontEnd.Site.Models.Services;


namespace BeautySalon.FrontEnd.Site
{
    public class WebApiApplication : System.Web.HttpApplication
    {
		public static IMapper _mapper { get; private set; }

		protected void Application_Start()
        {
            
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
			// 初始化 AutoMapper
			ConfigureAutoMapper();

			
		}

		private void ConfigureAutoMapper()
		{
			var config = new MapperConfiguration(cfg =>
			{
				// 使用 MappingProfile
				cfg.AddProfile<MappingProfile>();
			});

			_mapper = config.CreateMapper();
		}
	}
}
