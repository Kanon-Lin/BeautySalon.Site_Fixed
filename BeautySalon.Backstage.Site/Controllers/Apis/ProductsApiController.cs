using BeautySalon.Backstage.Site.Models.Dtos;
using BeautySalon.Backstage.Site.Models.EFModels;
using BeautySalon.Backstage.Site.Models.Repositories;
using BeautySalon.Backstage.Site.Models.Services;
using BeautySalon.Backstage.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;

namespace BeautySalon.Backstage.Site.Controllers.Apis
{
    [RoutePrefix("api/ProductsApi")]
    public class ProductsApiController : ApiController
    { 
        [HttpGet]
        public IHttpActionResult ProductIndext()
        {
            var db = new AppDbContext();

            var data = db.Services
                .AsNoTracking()
                .Include(p => p.ServiceCategory)
                .OrderBy(p => p.ServiceCategory.CategoryID)
                .Select(p => new ProductVm
                {
                    ProductId = p.ServiceID,
                    ProductName = p.ServiceName,
                    CategoryId = p.CategoryID,
                    CategoryName = p.ServiceCategory.CategoryName,
                    Price = p.Price,
                    Duration = p.Duration,
                    Description = p.Description,

                })
                .ToList();

            return Ok(data);
        }

        [HttpGet]
        [Route("GetCategory")]
        public IHttpActionResult GetCategory()
        {
            var db = new AppDbContext();

            var category = db.ServiceCategories
                .AsNoTracking()
                .OrderBy(c => c.CategoryID)
                .Select(c => new ProductCategoryVm
                {
                    Id = c.CategoryID,
                    CategoryName= c.CategoryName
                })
                .ToList();

            return Ok(category);
        }

        [HttpPost]
        [Route("CreateProduct")]
        public IHttpActionResult CreateProduct(ProductVm vm)
        {
            var service = new ProductService();
            try
            {
                ProductDto dto = vm.ToDto();
                service.Create(dto);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetProduct/{id}")]
        public IHttpActionResult GetProduct(int id)
        {
            var repo = new ProductRepository();
            var product = repo.GetProductById(id);

            if (product == null)
            {
                return BadRequest("該商品ID不存在");
            }

            return Ok(product);
        }

        [HttpPut]
        [Route("UpdateProduct/{id}")]
        public IHttpActionResult UpdateProduct(int id, [FromBody] ProductVm vm)
        {
            if (vm == null)
            {
                return BadRequest("無法解析請求中的資料。");
            }

            var service = new ProductService();

            try
            {
                ProductDto dto = vm.ToDto();
                dto.ProductId = id;  // 將路由中的 id 賦值給 DTO

                service.UpdateProduct(dto);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteProduct/{id}")]
        public IHttpActionResult DeleteProduct(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var service = new ProductService();

            try
            {
                service.DeleteProduct(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("SearchProduct")]
        public IHttpActionResult SearchProduct(string name = null)
        {
            var db = new AppDbContext();

            IQueryable<Service> data = db.Services;
            if (!string.IsNullOrEmpty(name))
            {
                data = data.Where(p => p.ServiceName.Contains(name));
            }

            var query = data
                .Include(p => p.ServiceCategory)
                .OrderBy(p => p.CategoryID)
                .Select(p => new ProductVm
                {
                    ProductId = p.ServiceID,
                    ProductName = p.ServiceName,
                    CategoryName = p.ServiceCategory.CategoryName,
                    Price = p.Price,
                    Duration = p.Duration,
                    Description = p.Description,
                })
                .ToList();

            return Ok(query);
        }
    }
}
