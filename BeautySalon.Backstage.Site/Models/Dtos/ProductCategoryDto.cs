using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySalon.Backstage.Site.Models.Dtos
{
    public class ProductCategoryDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
}