using BeautySalon.Backstage.Site.Models.Dtos;
using BeautySalon.Backstage.Site.Models.EFModels;
using BeautySalon.Backstage.Site.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySalon.Backstage.Site.Models.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private AppDbContext _db;
        public ProductRepository()
        {
            _db = new AppDbContext();
        }
        public ProductRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Create(ProductDto dto)
        {
            _db.Services.Add(new Service
            {
                ServiceName = dto.ProductName,
                CategoryID = dto.CategoryId,
                Price = dto.Price,
                Duration = dto.Duration,
                Description = dto.Description,
                CreatedDate = dto.CreatedDate,
            });

            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var db = new AppDbContext();

            var product = db.Services.FirstOrDefault(p => p.ServiceID == id);

            if (product == null) throw new Exception("找不到該服務項目");

            db.Services.Remove(product);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("刪除時發生錯誤", ex);
            }
        }

        public ProductDto Get(int id)
        {
            var db = new AppDbContext();

            var product = db.Services
                .AsNoTracking()
                .Where(p => p.ServiceID == id)
                .Select(p => new ProductDto
                {
                    ProductId = p.ServiceID,
                    ProductName = p.ServiceName,
                    CategoryId = p.CategoryID,
                    Price = p.Price,
                    Duration = p.Duration,
                    Description = p.Description,
                    CreatedDate = p.CreatedDate,
                })
                .FirstOrDefault();
            if (product == null) return null;

            return product;
        }

        public bool IsProductExist(string productName)
        {
            var name = _db.Services
                .AsNoTracking()
                .FirstOrDefault(n => n.ServiceName == productName);
            if (name != null) { return true; }
            return false;
        }

        public void Update(ProductDto dto)
        {
            using (var db = new AppDbContext())
            {
                // 從數據庫中查找要更新的服務
                var product = db.Services.FirstOrDefault(p => p.ServiceID == dto.ProductId);

                // 檢查產品是否存在
                if (product == null)
                {
                    throw new Exception("要更新的資料不存在");
                }

                // 更新實體屬性
                product.ServiceName = dto.ProductName;
                product.CategoryID = dto.CategoryId;
                product.Price = dto.Price;
                product.Duration = dto.Duration;
                product.Description = dto.Description;

                // 標記為已修改
                db.Entry(product).State = System.Data.Entity.EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception("更新資料時發生錯誤", ex);
                }
            }
        }


        internal object GetProductById(int id)
        {
            var product = _db.Services
                .AsNoTracking()
                .Where(p => p.ServiceID == id)
                .Select(p => new ProductDto
                {
                    ProductId = p.ServiceID,
                    ProductName = p.ServiceName,
                    CategoryId = p.CategoryID,
                    Price = p.Price,
                    Duration = p.Duration,
                    Description = p.Description,
                    CreatedDate = p.CreatedDate,
                })
                .FirstOrDefault();

            return product;
        }
    }
}