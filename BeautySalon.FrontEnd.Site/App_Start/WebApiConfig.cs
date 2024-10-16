using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BeautySalon.FrontEnd.Site.Models.Infra;

namespace BeautySalon.FrontEnd.Site
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API 設定和服務
			// Web API 路由
			config.MapHttpAttributeRoutes();
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			// 配置 JWT 驗證
			ConfigureJwtAuthentication(config);

			// 配置認證
			config.Filters.Add(new AuthorizeAttribute());

#if DEBUG
			config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
#endif

			// 配置 CORS（如果需要）
			//var cors = new EnableCorsAttribute("*", "*", "*");
			//config.EnableCors(cors);
		}

		private static void ConfigureJwtAuthentication(HttpConfiguration config)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("adfhaertg123485earhbzdfaerasdfgaaerygahnerghnnerhnasdfcvrtujtrn")),
				ValidateIssuer = false,
				ValidateAudience = false,
				ClockSkew = TimeSpan.Zero,
				RequireExpirationTime = true,
				ValidateLifetime = true,
				NameClaimType = System.Security.Claims.ClaimTypes.Name,
				RoleClaimType = System.Security.Claims.ClaimTypes.Role
			};

			config.MessageHandlers.Add(new JwtAuthenticationHandler(tokenValidationParameters));
		}
	}
}