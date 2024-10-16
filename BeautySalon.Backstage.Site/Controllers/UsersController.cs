using BeautySalon.Backstage.Site.Models.EFModels;
using BeautySalon.Backstage.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BeautySalon.Backstage.Site.Controllers
{
    public class UsersController : Controller
    {
        private AppDbContext _db = new AppDbContext();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVm vm)
        {
            if (ModelState.IsValid)
            {
                if (!IsValid(vm.EmployeeNo, vm.Password))
                {
                    ModelState.AddModelError("", "帳號或密碼錯誤");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            // 如果帳號密碼正確, 則建立一個登入用的 Cookie
            HttpCookie cookie;
            var returnUrl = ProcessLogin(vm.EmployeeNo, false, out cookie);
            Response.Cookies.Add(cookie);

            // 檢查是否有返回的 URL，否則重定向到首頁
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                returnUrl = Url.Action("Index", "Home"); // 指向首頁
            }

            return Redirect(returnUrl);
        }

        private string ProcessLogin(string employeeNo, bool rememberMe, out HttpCookie cookie)
        {
            var employee = _db.Employees.FirstOrDefault(u => u.EmployeeNo == employeeNo);
            if (employee == null)
            {
                throw new InvalidOperationException("使用者不存在");
            }

            //string role = "employee"; // 根據實際需求設定角色
            string nickname = employee.Nickname; // `Nickname` 是暱稱的欄位

            // 建立認證票
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,                                      // 版本號
                employeeNo,                             // 使用者名稱
                DateTime.Now,                           // 發行時間
                DateTime.Now.AddDays(2),                // 到期時間
                rememberMe,                             // 是否持久化
                nickname,                                   // 使用者角色或其他數據
                "/"                                     // Cookie 路徑
            );

            // 加密認證票
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            // 建立 Cookie 並加入認證票
            cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            if (rememberMe)
            {
                cookie.Expires = ticket.Expiration;
            }

            // 取得 return URL
            string url = FormsAuthentication.GetRedirectUrl(employeeNo, rememberMe);

            return url;
        }

        private bool IsValid(string employeeNo, string password)
        {
            
            var employee = _db.Employees.FirstOrDefault(x => x.EmployeeNo == employeeNo && x.Password == password);
            return (employee != null);
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }
    }
}