using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BeautySalon.FrontEnd.Site.Startup))]

namespace BeautySalon.FrontEnd.Site
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 配置 OWIN 服務，例如 SignalR、身份驗證中介軟件等
        }

    }
}