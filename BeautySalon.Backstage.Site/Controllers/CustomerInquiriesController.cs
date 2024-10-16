using BeautySalon.Backstage.Site.Models.EFModels;
using BeautySalon.Backstage.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeautySalon.Backstage.Site.Controllers
{
    public class CustomerInquiriesController : Controller
    {
        private readonly AppDbContext _db;

        public CustomerInquiriesController(AppDbContext db)
        {
            _db = db;
        }

        public CustomerInquiriesController() : this(new AppDbContext()) { }

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                var inquiries = _db.CustomerInquiries
                    .OrderByDescending(i => i.InquiryDate)
                    .Select(i => new CustomerInquiryVm
                    {
                        InquiryID = i.InquiryID,
                        Name = i.Name,
                        Gender = i.Gender,
                        Email = i.Email,
                        TelephoneNumber = i.TelephoneNumber,
                        Message = i.Message,
                        InquiryDate = i.InquiryDate
                    }).ToList();

                return View(inquiries);
            }
            catch (Exception ex)
            {
                // 記錄錯誤或返回錯誤視圖
                return View("Error", new HandleErrorInfo(ex, "CustomerInquiries", "Index"));
            }
        }
    }
}