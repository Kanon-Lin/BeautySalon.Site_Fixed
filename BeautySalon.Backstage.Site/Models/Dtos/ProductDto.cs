using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySalon.Backstage.Site.Models.Dtos
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}