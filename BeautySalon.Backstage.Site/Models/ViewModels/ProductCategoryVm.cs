using BeautySalon.Backstage.Site.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySalon.Backstage.Site.Models.ViewModels
{
    public class ProductCategoryVm
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }

    public static class CategoryExt
    {
        public static ProductCategoryDto ToDto(this ProductCategoryVm vm)
        {
            return new ProductCategoryDto
            {
                Id = vm.Id,
                CategoryName = vm.CategoryName,
                Description = vm.Description,
            };
        }
    }
}