using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySalon.FrontEnd.Site.Models.ViewModels
{
	public class ProductVm
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public int Duration { get; set; }
		public int Quantity { get; set; }
	}
}