using BeautySalon.FrontEnd.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BeautySalon.FrontEnd.Site.Controllers.APIs
{
    public class CustomerInquiryApiController : ApiController
    {
        private readonly AppDbContext _db = new AppDbContext();
        // POST: api/CustomerInquiryApi


        [HttpPost]
        [AllowAnonymous] // 確保不需要身份驗證
        public async Task<IHttpActionResult> PostInquiry([FromBody] CustomerInquiry model)
        {
            if (model == null)
            {
                return BadRequest("無效的資料。");
            }

            if (!ModelState.IsValid)
            {
                // 收集所有驗證錯誤
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToArray();
                // 使用 Content 方法返回自訂的回應
                return Content(HttpStatusCode.BadRequest, new { success = false, message = "提交失敗，請檢查輸入的資料。", errors });
            }

            model.InquiryDate = System.DateTime.Now;
            _db.CustomerInquiries.Add(model);
            await _db.SaveChangesAsync();

            return Ok(new { success = true, message = "提交成功" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}