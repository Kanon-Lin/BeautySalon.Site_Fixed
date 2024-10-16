using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BeautySalon.Backstage.Site.Models.EFModels;
using BeautySalon.Backstage.Site.Models.ViewModels;

namespace BeautySalon.Backstage.Site.Controllers
{
    public class MembersController : Controller
    {
        private readonly AppDbContext _db; // 替換為你的資料上下文類別

        public MembersController(AppDbContext db)
        {
            _db = db;
        }
        // 無參數建構函式
        public MembersController() : this(new AppDbContext())
        {

        }

        public ActionResult Index(string searchTerm)
        {
            var filteredMembers = string.IsNullOrEmpty(searchTerm)
                ? _db.Members.ToList() // 從資料庫讀取所有會員
                : _db.Members
                .Where(m => m.Name.Contains(searchTerm) || m.PhoneNumber.Contains(searchTerm))
                .ToList();

            var viewModel = filteredMembers.Select(m => new MemberVm
            {
                MemberID = m.MemberID,
                Name = m.Name,
                Gender = m.Gender,
                PhoneNumber = m.PhoneNumber,
                Email = m.Email,
                Account = m.Account,
                RegistrationDate = m.RegistrationDate
            });

            return View(viewModel);
        }


    }
}