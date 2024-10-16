using BeautySalon.Backstage.Site.Models.EFModels;
using BeautySalon.Backstage.Site.Models.Services;
using BeautySalon.Backstage.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeautySalon.Backstage.Site.Controllers
{
    public class AppointmentsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        private readonly AppDbContext _db;

        public AppointmentsController(AppDbContext db)
        {
            _db = db;
        }
        public AppointmentsController() : this(new AppDbContext()) { }

        
        [HttpGet]
        //Index顯示預約清單
        public ActionResult Index(string searchTerm)
        {
            var filteredAppointments = string.IsNullOrEmpty(searchTerm)
                ? _db.Appointments.OrderByDescending(a => a.AppointmentStart).ToList().ToList()
                : _db.Appointments
                .Where(a => a.AppointmentID.ToString().Contains(searchTerm)
                || a.MemberID.ToString().Contains(searchTerm)
                || a.EmployeeID.ToString().Contains(searchTerm)
                )
                .OrderByDescending(a => a.AppointmentStart)
                .ToList();

            var viewModel = filteredAppointments.Select(a => new AppointmentVm
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
                Nickname = a.Employee.Nickname,//來自員工的暱稱
                Name = a.Member.Name,
            });

            return View(viewModel);
        }


        [HttpPost]
        [Route("CancelAppointment/{appointmentId}")]
        public ActionResult CancelAppointment(int appointmentId)
        {
            var service = new AppointmentService();

            // 獲取訂單詳細 ID
            int orderDetailId = service.GetOrderDetailId(appointmentId);

            // 取消預約
            service.CancelAppointment(appointmentId);

            // 更新數量
            service.UpdateQuantity(orderDetailId);

            return Json(new { success = true });
        }
    }
}