using BeautySalon.Backstage.Site.Models.Dtos;
using BeautySalon.Backstage.Site.Models.Interfaces;
using BeautySalon.Backstage.Site.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySalon.Backstage.Site.Models.Services
{
    public class ProductService
    {
        private IProductRepository _repo;
        public ProductService()
        {
            _repo = new ProductRepository();
        }
        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        internal void Create(ProductDto dto)
        {
            //bool isProductExist = _repo.IsProductExist(dto.ProductName);
            //if (isProductExist) { throw new Exception("服務項目名稱已存在"); }
            if (dto.CategoryId == 0)
            {
                throw new Exception("請選擇服務類別");
            }
            if (dto.Duration % 30 != 0)
            {
                throw new Exception("施作時長請以30分鐘為單位");
            }

            dto.CreatedDate = DateTime.Now;
            _repo.Create(dto);
        }

        internal void DeleteProduct(int id)
        {
            var product = new ProductDto();
            product.ProductId = id;

            _repo.Delete(id);
        }

        internal void UpdateProduct(ProductDto dto)
        {
            ProductDto productInDb = _repo.Get(dto.ProductId);

            if (productInDb == null)
            {
                throw new Exception($"無法找到ID為 {dto.ProductId} 的產品");
            }

            productInDb.ProductName = dto.ProductName;
            productInDb.CategoryId = dto.CategoryId;
            productInDb.Duration = dto.Duration;
            productInDb.Price = dto.Price;
            productInDb.Description = dto.Description;

            _repo.Update(productInDb);
        }
    }
}