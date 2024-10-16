using BeautySalon.Backstage.Site.Models.Dtos;
using BeautySalon.Backstage.Site.Models.EFModels;
using BeautySalon.Backstage.Site.Models.Services;
using BeautySalon.Backstage.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BeautySalon.Backstage.Site.Controllers.Apis
{
    [RoutePrefix("api/CategoriesApi")]
    public class CategoriesApiController : ApiController
    {
        [HttpGet]
        public IHttpActionResult CategoryIndex()
        {
            var db = new AppDbContext();

            var data = db.ServiceCategories
                         .AsNoTracking()
                         .Select(c => new ProductCategoryVm
                         {
                             Id = c.CategoryID,
                             CategoryName = c.CategoryName,
                             Description = c.Description
                         })
                         .ToList();

            return Ok(data);
        }

        [HttpPost]
        [Route("CreateCategory")]
        public IHttpActionResult CreateCategory(ProductCategoryVm vm)
        {
            var service = new ProductCategoryService();
            try
            {
                ProductCategoryDto dto = vm.ToDto();
                service.CreateCategory(dto);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateCategory/{id}")]
        public IHttpActionResult UpdateCategory(int id,[FromBody] ProductCategoryVm vm)
        {
            var service = new ProductCategoryService();
            try
            {
                ProductCategoryDto dto = vm.ToDto();
                dto.Id = id;

                service.UpdateCategory(dto);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("DeleteCategory")]
        public IHttpActionResult DeleteCategory([FromBody] CategoryRequest request)
        {
            if (request == null || request.categoryId <= 0)
            {
                return BadRequest("無效的類別 ID。");
            }

            var service = new ProductCategoryService();
            try
            {
                service.DeleteCategory(request.categoryId);
                return Ok(new { success = true, message = "類別已成功刪除。" });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("SearchCategory")]
        public IHttpActionResult SearchCategory(string name = null)
        {
            var db = new AppDbContext();

            IQueryable<ServiceCategory> data = db.ServiceCategories;
            if (!string.IsNullOrEmpty(name))
            {
                data = data.Where(c => c.CategoryName.Contains(name));
            }

            var query = data
                .OrderBy(c => c.CategoryID)
                .Select(c => new ProductCategoryVm
                {
                    Id = c.CategoryID,
                    CategoryName = c.CategoryName,
                    Description = c.Description
                })
                .ToList();

            return Ok(query);
        }

        public class CategoryRequest
        {
            public int categoryId { get; set; }
        }




        [HttpGet]
        [Route("GetCategory/{id}")]
        public IHttpActionResult GetCategory(int id)
        {
            var db = new AppDbContext();
            var category = db.ServiceCategories
                             .Where(c => c.CategoryID == id)
                             .Select(c => new ProductCategoryVm
                             {
                                 Id = c.CategoryID,
                                 CategoryName = c.CategoryName,
                                 Description = c.Description
                             })
                             .FirstOrDefault();

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }
    }
}
