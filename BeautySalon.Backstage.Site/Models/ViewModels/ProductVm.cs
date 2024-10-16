using BeautySalon.Backstage.Site.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySalon.Backstage.Site.Models.ViewModels
{
    public class ProductVm
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

    public static class ProductExt
    {
        public static ProductDto ToDto(this ProductVm vm)
        {
            return new ProductDto
            {
                ProductId = vm.ProductId,
                ProductName = vm.ProductName,
                CategoryId = vm.CategoryId,
                Price = vm.Price,
                Duration = vm.Duration,
                Description = vm.Description,
                CreatedDate = vm.CreatedDate,
            };
        }
    }
}