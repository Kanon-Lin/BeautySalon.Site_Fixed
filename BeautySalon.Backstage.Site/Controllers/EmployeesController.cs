using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BeautySalon.Backstage.Site.Models.EFModels;
using BeautySalon.Backstage.Site.Models.ViewModels;

namespace BeautySalon.Backstage.Site.Controllers
{
    public class EmployeesController : Controller
    {
        private AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeNo,Name,Nickname,Password,Gender,Email,Phone")] EmployeeCreateVm employee, HttpPostedFileBase Image)
        {
            employee.CreatedDate = DateTime.Now;
            employee.RoleGroupID = 2;

            // 驗證唯一性
            ValidateUniqueFields(employee);

            // 檢查模型狀態
            if (!ModelState.IsValid)
                return View(employee);
            try
            {
                var newEmployee = new Employee
                {
                    EmployeeNo = employee.EmployeeNo,
                    Name = employee.Name,
                    Nickname = employee.Nickname,
                    Password = employee.Password,
                    Gender = employee.Gender,
                    Email = employee.Email,
                    Phone = employee.Phone,
                    RoleGroupID = employee.RoleGroupID,
                    CreatedDate = employee.CreatedDate,
                    //Image = employee.Image
                };

                // 使用 Add 方法添加並保存到數據庫
                db.Employees.Add(newEmployee);
                db.SaveChanges();

                // 儲存成功後，清除模型狀態中的錯誤
                ModelState.Clear(); // 清除所有錯誤

                // 儲存成功後，使用 TempData 顯示提醒訊息
                TempData["SuccessMessage"] = "儲存成功！";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                // 直接處理異常並顯示通用錯誤消息
                ModelState.AddModelError("", "儲存資料時發生錯誤，請稍後再試。");
                return View(employee);
            }
        }


        public ActionResult Edit(int id)
        {
            var employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

            var employeeUpdateVm = new EmployeeUpdateVm
            {
                EmployeeID = employee.EmployeeID,
                EmployeeNo = employee.EmployeeNo,
                Name = employee.Name,
                Nickname = employee.Nickname,
                Gender = employee.Gender,
                Email = employee.Email,
                Phone = employee.Phone,
                Image = employee.Image,
                Password = employee.Password,
                CreatedDate = employee.CreatedDate,
            };

            return View(employeeUpdateVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,Name,Nickname,Password,Gender,Email,Phone,Image")] EmployeeUpdateVm employee)
        {
            if (ModelState.IsValid)
            {
                // 使用 Find 方法尋找現有員工
                var existingEmployee = db.Employees.Find(employee.EmployeeID);

                if (existingEmployee != null)
                {
                    // 僅更新需要的欄位
                    existingEmployee.Name = employee.Name;
                    existingEmployee.Nickname = employee.Nickname;

                    // 只有當密碼不為空時才更新
                    if (!string.IsNullOrEmpty(employee.Password))
                    {
                        existingEmployee.Password = employee.Password; // 更新密碼
                    }

                    existingEmployee.Gender = employee.Gender;
                    existingEmployee.Email = employee.Email;
                    existingEmployee.Phone = employee.Phone;

                    //// 如果 Image 有提供，則更新圖片
                    //if (!string.IsNullOrEmpty(employee.Image))
                    //{
                    //    existingEmployee.Image = employee.Image;
                    //}

                    // 確保 email 和 phone 的唯一性
                    if (!string.IsNullOrEmpty(employee.Email))
                    {
                        var emailExists = db.Employees.Any(e => e.Email == employee.Email && e.EmployeeID != employee.EmployeeID);
                        if (emailExists)
                        {
                            ModelState.AddModelError("Email", "電子郵件已存在。");
                        }
                    }

                    if (!string.IsNullOrEmpty(employee.Phone))
                    {
                        var phoneExists = db.Employees.Any(e => e.Phone == employee.Phone && e.EmployeeID != employee.EmployeeID);
                        if (phoneExists)
                        {
                            ModelState.AddModelError("Phone", "電話號碼已存在。");
                        }
                    }

                    // 如果有 ModelState 錯誤，返回原視圖顯示錯誤
                    if (!ModelState.IsValid)
                    {
                        // 確保從資料庫中重新獲取 EmployeeNo 和 CreateDate 的值
                        var employeeFromDb = db.Employees.Find(employee.EmployeeID);
                        if (employeeFromDb != null)
                        {
                            employee.EmployeeNo = employeeFromDb.EmployeeNo;
                            employee.CreatedDate = employeeFromDb.CreatedDate;
                        }


                        return View(employee); // 返回視圖時帶上這些欄位
                    }

                    // 儲存變更
                    try
                    {
                        db.SaveChanges(); // 儲存變更


                        // 儲存成功後，清除模型狀態中的錯誤
                        ModelState.Clear(); // 清除所有錯誤

                        // 儲存成功後，使用 TempData 顯示提醒訊息
                        TempData["SuccessMessage"] = "儲存成功！";
                        return RedirectToAction("Index");

                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "無法儲存變更: " + ex.Message); // 捕捉儲存異常
                    }
                }
                else
                {
                    ModelState.AddModelError("", "找不到指定的員工。"); // 找不到員工的錯誤提示
                }
            }

            return View(employee); // 返回原視圖以顯示錯誤信息
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        // 驗證唯一性方法
        private void ValidateUniqueFields(EmployeeCreateVm employee)
        {
            bool isValid = true;

            if (!IsEmployeeNoUnique(employee.EmployeeNo))
            {
                ModelState.AddModelError("EmployeeNo", "員工編號已存在，請使用其他員工編號。");
                isValid = false; // 設定為無效
            }
            else
            {
                // 清除舊的錯誤訊息
                ModelState.Remove("EmployeeNo");
            }

            if (!string.IsNullOrWhiteSpace(employee.Email) && !IsEmailUnique(employee.Email))
            {
                ModelState.AddModelError("Email", "電子郵件已存在，請使用其他電子郵件。");
                isValid = false; // 設定為無效
            }
            else
            {
                // 清除舊的錯誤訊息
                ModelState.Remove("Email");
            }

            if (!string.IsNullOrWhiteSpace(employee.Phone) && !IsPhoneUnique(employee.Phone))
            {
                ModelState.AddModelError("Phone", "電話已存在，請使用其他電話。");
                isValid = false; // 設定為無效
            }
            else
            {
                // 清除舊的錯誤訊息
                ModelState.Remove("Phone");
            }

            // 如果全部唯一性檢查成功，則可更新狀態
            if (isValid)
            {
                ModelState.Clear(); // 清除所有錯誤
            }
        }

        // 通用唯一性檢查
        private bool IsUnique<T>(Func<T, bool> checkFunc, T value) =>
            !db.Employees.Any(e => checkFunc(value));
        private bool IsEmployeeNoUnique(string employeeNo)
        {
            return !db.Employees.Any(e => e.EmployeeNo == employeeNo);
        }
        private bool IsEmailUnique(string email)
        {
            return !db.Employees.Any(e => e.Email == email);
        }
        private bool IsPhoneUnique(string phone)
        {
            return !db.Employees.Any(e => e.Phone == phone);
        }
        public JsonResult CheckEmployeeNo()
        {
            // 從請求中獲取 employeeNo 和 id 參數
            string employeeNo = Request.QueryString["employeeNo"];
            int? id = Request.QueryString["id"] != null ? (int?)Convert.ToInt32(Request.QueryString["id"]) : null;

            // 如果是編輯模式 (id 有值)，直接返回 true，不進行檢查
            if (id.HasValue)
            {
                return Json(true, JsonRequestBehavior.AllowGet); // 返回 true，表示不進行檢查
            }

            // 在新增模式下進行檢查
            if (!string.IsNullOrEmpty(employeeNo))
            {
                // 檢查其他員工是否已經存在該編號
                var existingEmployee = db.Employees.FirstOrDefault(e => e.EmployeeNo == employeeNo);
                if (existingEmployee != null)
                {
                    return Json(false, JsonRequestBehavior.AllowGet); // 响应 false，表示編號已存在
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet); // 响应 true，表示編號可用
        }
        public JsonResult CheckEmployeeDetails()
        {
            // 從請求中獲取 phone 和 email 參數
            string phone = Request.QueryString["phone"];
            string email = Request.QueryString["email"];
            int? employeeID = Request.QueryString["EmployeeID"] != null ? (int?)Convert.ToInt32(Request.QueryString["EmployeeID"]) : null;

            // 檢查 Phone
            if (!string.IsNullOrEmpty(phone))
            {
                // 檢查資料庫中是否有其他員工的電話號碼與輸入相同
                var existingPhone = db.Employees.FirstOrDefault(e => e.Phone == phone && e.EmployeeID != employeeID);
                if (existingPhone != null)
                {
                    return Json(false, JsonRequestBehavior.AllowGet); // 响应 false，表示電話已存在
                }
            }

            // 檢查 Email
            if (!string.IsNullOrEmpty(email))
            {
                // 檢查資料庫中是否有其他員工的電子郵件與輸入相同
                var existingEmail = db.Employees.FirstOrDefault(e => e.Email == email && e.EmployeeID != employeeID);
                if (existingEmail != null)
                {
                    return Json(false, JsonRequestBehavior.AllowGet); // 响应 false，表示電子郵件已存在
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet); // 响应 true，表示電話和電子郵件可用
        }

    }
}