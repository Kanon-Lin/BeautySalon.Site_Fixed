using BeautySalon.Backstage.Site.Models.Dtos;
using BeautySalon.Backstage.Site.Models.EFModels;
using BeautySalon.Backstage.Site.Models.Interfaces;
using BeautySalon.Backstage.Site.Models.Repositories;
using BeautySalon.Backstage.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySalon.Backstage.Site.Models.Services
{
    public class ProductCategoryService
    {
        private IProductCategoryRepository _repo;

        public ProductCategoryService()
        {
            _repo = new ProductCategoryRepository();
        }
        public ProductCategoryService(IProductCategoryRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<ProductCategoryDto> GetAllCategories()
        {
            var category = _repo.GetAll();

            if (category == null)
            {
                throw new Exception("未找到任何產品類別");
            }

            return category;
        }

        public void CreateCategory(ProductCategoryDto dto)
        {
            var categoryName = new ServiceCategory().CategoryName;
            if (dto.CategoryName == categoryName)
            {
                throw new Exception("名稱已存在");
            }

            var category = new ServiceCategory
            {
                CategoryName = dto.CategoryName,
                Description = dto.Description
            };

            _repo.Create(category);
        }

        public ProductCategoryDto GetCategory(int id)
        {
            var category = _repo.Get(id); // 使用 id 查詢類別
            if (category == null) return null;

            return WebApiApplication._mapper.Map<ProductCategoryDto>(category); // 返回 DTO
        }

        internal void UpdateCategory(ProductCategoryDto dto)
        {
            ProductCategoryDto CategoryInDb = _repo.Get(dto.Id);

            CategoryInDb.CategoryName = dto.CategoryName;
            CategoryInDb.Description = dto.Description;

            _repo.Update(CategoryInDb);
        }

        internal void DeleteCategory(int id)
        {
            _repo.Delete(id);
        }
    }
}