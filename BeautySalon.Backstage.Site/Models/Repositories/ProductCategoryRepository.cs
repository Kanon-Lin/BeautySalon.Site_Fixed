using BeautySalon.Backstage.Site.Models.Dtos;
using BeautySalon.Backstage.Site.Models.EFModels;
using BeautySalon.Backstage.Site.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace BeautySalon.Backstage.Site.Models.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private AppDbContext _db;

        public ProductCategoryRepository()
        {
            _db = new AppDbContext();
        }
        public ProductCategoryRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Create(ServiceCategory category)
        {
            _db.ServiceCategories.Add(new ServiceCategory
            {
                CategoryID = category.CategoryID,
                CategoryName = category.CategoryName,
                Description = category.Description,
            });

            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var db = new AppDbContext();

            var category = db.ServiceCategories.FirstOrDefault(c => c.CategoryID == id);

            if (category == null) throw new Exception("找不到該類別");

            db.ServiceCategories.Remove(category);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("刪除時發生錯誤", ex);
            }
        }

        public ProductCategoryDto Get(int id)
        {
            var db = new AppDbContext();
            var category = db.ServiceCategories
                .AsNoTracking()
                .Where(c => c.CategoryID == id)
                .Select(c => new ProductCategoryDto
                {
                    Id = c.CategoryID,
                    CategoryName = c.CategoryName,
                    Description = c.Description,
                })
                .First();
            if (category == null) return null;

            return WebApiApplication._mapper.Map<ProductCategoryDto>(category);
        }

        public IEnumerable<ProductCategoryDto> GetAll()
        {
            var data = _db.ServiceCategories
                .AsNoTracking()
                .OrderBy(c => c.CategoryID)
                .Select(c => new ProductCategoryDto
                {
                    Id = c.CategoryID,
                    CategoryName = c.CategoryName,
                    Description = c.Description
                })
                .ToList();

            return data;
        }

        public void Update(ProductCategoryDto dto)
        {

            var category = _db.ServiceCategories.SingleOrDefault(c => c.CategoryID == dto.Id);
            if (category == null)
            {
                throw new Exception("要更新的資料不存在。");
            }

            // 使用 AutoMapper 將 DTO 轉換為實體
            WebApiApplication._mapper.Map(dto, category);

            // 設置狀態為 Modified
            _db.Entry(category).State = System.Data.Entity.EntityState.Modified;

            // 儲存更改
            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new InvalidOperationException("資料庫中的資料可能已經被其他使用者修改。");
            }

        }
    }
}