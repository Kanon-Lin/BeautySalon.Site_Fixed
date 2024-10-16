using BeautySalon.Backstage.Site.Models;
using BeautySalon.Backstage.Site.Models.EFModels;
using BeautySalon.Backstage.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeautySalon.Backstage.Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        // 構造函數，用於依賴注入資料庫上下文
        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public HomeController() : this(new AppDbContext()) { }

        [HttpGet]
        public ActionResult Index(string searchTerm, DateTime? selectedDate)
        {
            // 設定選擇的日期，若未選擇則使用今天的日期
            var dateToDisplay = selectedDate ?? DateTime.Today;

            // 過濾訂單狀態為1和3的訂單
            var filteredOrders = _db.Orders
                .Where(o => (o.OrderStatus == 1 || o.OrderStatus == 3) &&
                            (string.IsNullOrEmpty(searchTerm) || o.OrderID.ToString().Contains(searchTerm) || o.MemberID.ToString().Contains(searchTerm)))
                .ToList();

            var orderViewModel = filteredOrders.Select(o => new OrderVm
            {
                OrderID = o.OrderID,
                MemberID = o.MemberID,
                OrderStatus = o.OrderStatus,
                OrderDate = o.OrderDate,
                TotalQuantity = o.TotalQuantity,
                TotalAmount = o.TotalAmount,
                TotalNetAmount = o.TotalNetAmount,
                DiscountedAmount = o.DiscountedAmount,
                CancelDate = o.CancelDate,
                SumRemainingQuantity = o.SumRemainingQuantity,
                Name = o.Member.Name
            });

            // 過濾預約狀態為1且 AppointmentStart 是所選日期的預約
            var filteredAppointments = _db.Appointments
     .Where(a => a.AppointmentStatus == 1 &&
                 DbFunctions.TruncateTime(a.AppointmentStart) == dateToDisplay.Date &&
                 (string.IsNullOrEmpty(searchTerm) || a.AppointmentID.ToString().Contains(searchTerm)
                 || a.MemberID.ToString().Contains(searchTerm)
                 || a.EmployeeID.ToString().Contains(searchTerm)))
     .OrderBy(a => a.AppointmentStart) // 根據 AppointmentStart 排序
     .ToList();


            var appointmentViewModel = filteredAppointments.Select(a => new AppointmentVm
            {
                AppointmentID = a.AppointmentID,
                MemberID = a.MemberID,
                OrderID = a.OrderID,
                OrderDetailID = a.OrderDetailID,
                ServiceID = a.ServiceID,
                EmployeeID = a.EmployeeID,
                AppointmentStart = a.AppointmentStart,
                AppointmentEnd = a.AppointmentEnd,
                AppointmentStatus = a.AppointmentStatus,
                ServiceName = a.Service.ServiceName,
                Nickname = a.Employee.Nickname,
                Name = a.Member.Name,
            }).ToList();

            // 將訂單和預約清單加入HomePageVm
            var viewModel = new HomePageVm
            {
                Orders = orderViewModel,
                Appointments = appointmentViewModel,
                SelectedDate = dateToDisplay // 設定所選日期
            };

            return View(viewModel);
        }




    }
}
