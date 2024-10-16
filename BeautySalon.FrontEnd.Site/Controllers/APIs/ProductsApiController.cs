using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using BeautySalon.FrontEnd.Site.Models.ViewModels;
using BeautySalon.FrontEnd.Site.Models.EFModels;

namespace BeautySalon.FrontEnd.Site.Controllers.APIs
{
	[RoutePrefix("api/ProductsApi")]
	public class ProductsApiController : ApiController
	{

		[AllowAnonymous]
		[Route("GetAll")]
		public IHttpActionResult GetAll()
		{
			var db = new AppDbContext();

			var data = db.Services
				.AsNoTracking()
				.Include(s => s.ServiceCategory)
				.OrderBy(s => s.ServiceCategory.CategoryID)
				.GroupBy(s => s.ServiceCategory.CategoryName)
				.Select(c => new ProductCategoryVm
				{
					CategoryName = c.Key,
					Items = c.Select(p => new ProductVm
					{
						Id = p.ServiceID,
						Name = p.ServiceName,
						Price = p.Price,
						Duration = p.Duration,
						Quantity = 0

					})
				})
				.ToList();

			return Ok(data);
		}
	}
}
