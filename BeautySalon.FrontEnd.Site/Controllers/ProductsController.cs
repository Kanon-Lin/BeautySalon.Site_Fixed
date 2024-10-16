using BeautySalon.FrontEnd.Site.Models.EFModels;
using BeautySalon.FrontEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace BeautySalon.FrontEnd.Site.Controllers
{
    public class ProductsController : Controller
    {
		// GET: Products
		[HttpGet]
		public JsonResult Info()
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
						Name = p.ServiceName,
						Price = p.Price,
						Duration = p.Duration,
						Quantity = 0

					})
				})
				.ToList();

			return Json(data, JsonRequestBehavior.AllowGet);
		}
	}
}