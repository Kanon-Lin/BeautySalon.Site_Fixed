using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySalon.FrontEnd.Site.Models.ViewModels
{
	public class ProductCategoryVm
	{
		public int Id { get; set; }
		public string CategoryName { get; set; }
		public IEnumerable<ProductVm> Items { get; set; }
	}
}