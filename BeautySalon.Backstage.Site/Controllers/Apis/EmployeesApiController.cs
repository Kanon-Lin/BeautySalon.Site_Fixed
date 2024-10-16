using BeautySalon.Backstage.Site.Models.EFModels;
using BeautySalon.Backstage.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BeautySalon.Backstage.Site.Controllers.Apis
{
    [RoutePrefix("api/EmployeesApi")]
    public class EmployeesApiController : ApiController
    {
        private readonly AppDbContext db;

        public EmployeesApiController()
        {
            db = new AppDbContext(); // 初始化 DbContext
        }

        [HttpGet]
        // GET: api/EmployeesApi
        public IHttpActionResult GetAll()
        {
            try
            {
                var data = db.Employees
                    .OrderBy(x => x.EmployeeNo)
                    .Select(x => new EmployeeVm
                    {
                        EmployeeID = x.EmployeeID,
                        EmployeeNo = x.EmployeeNo,
                        Name = x.Name,
                        Nickname = x.Nickname,
                        Password = x.Password,
                        Gender = x.Gender,
                        Email = x.Email,
                        Phone = x.Phone,
                        CreatedDate = x.CreatedDate
                    })
                    .ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); // 返回 500 錯誤
            }
        }

        // GET: api/EmployeesApi/5
        [HttpGet]
        [Route("search")] // 使用路由屬性來區分
                          //[ResponseType(typeof(Employee))]
        public IHttpActionResult Search(string query = null)
        {
            try
            {
                IQueryable<Employee> data = db.Employees;

                if (!string.IsNullOrEmpty(query))
                {
                    data = data.Where(e => e.Name.Contains(query) ||
                                           e.Nickname.Contains(query) ||
                                           e.EmployeeNo.Contains(query));
                }

                var queryResult = data
                    .OrderBy(e => e.EmployeeNo)
                    .Select(e => new
                    {
                        e.EmployeeNo,
                        e.Name,
                        e.Nickname,
                        e.Gender,
                        e.Phone,
                        e.Email
                    })
                    .ToList();

                return Ok(queryResult);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); // 返回 500 錯誤
            }
        }


    }
}
