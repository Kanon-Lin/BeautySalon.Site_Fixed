using BeautySalon.FrontEnd.Site.Models.EFModels;
using BeautySalon.FrontEnd.Site.Models.Interfaces;
using BeautySalon.FrontEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using BeautySalon.FrontEnd.Site.Models.DTO;

namespace BeautySalon.FrontEnd.Site.Models.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private AppDbContext _db;

        public AppointmentRepository()
        {
            _db = new AppDbContext();
        }
        public AppointmentRepository(AppDbContext db)
        {
            _db = db;
        }

        public void CancelAppointment(int appointmentId)
        {
            var db = new AppDbContext();

            var appointmentStatus = db.Appointments
                                     .FirstOrDefault(a => a.AppointmentID == appointmentId);

            if (appointmentStatus != null)
            {
                appointmentStatus.AppointmentStatus = 2;
            }

            db.SaveChanges();
        }

        public void CreateAppointment(AppointmentDto dto)
        {
            DateTime date;
            if (!DateTime.TryParse(dto.Date, out date))
            {
                throw new Exception("日期格式無效");
            }

            TimeSpan timeSlot;
            if (!TimeSpan.TryParse(dto.TimeSlot, out timeSlot))
            {
                throw new Exception("時間格式無效");
            }
            DateTime appointmentStart = date.Date + timeSlot;
            DateTime appointmentEnd = appointmentStart.AddMinutes(dto.Duration);

            var newApoointment = new Appointment
            {
                MemberID = dto.MemberId,
                OrderID = dto.OrderId,
                OrderDetailID = dto.OrderDetailId,
                EmployeeID = dto.BeauticianId,
                ServiceID = dto.ServiceId,
                AppointmentStart = appointmentStart,
                AppointmentEnd = appointmentEnd,
                AppointmentStatus = 1
            };

            _db.Appointments.Add(newApoointment);
            _db.SaveChanges();
        }

        public List<Appointment> GetAppointment(int beauticianId, DateTime date)
        {
            return _db.Appointments
                      .AsNoTracking()
                      .Where(a => a.EmployeeID == beauticianId && DbFunctions.TruncateTime(a.AppointmentStart) == date.Date && a.AppointmentStatus == 1)
                      .ToList();
        }

        public List<BookedSlot> GetBookedTimeSlots(int beauticianId, DateTime date)
        {
            return _db.Appointments
                      .Where(a => a.EmployeeID == beauticianId
                         && DbFunctions.TruncateTime(a.AppointmentStart) == date.Date
                         && a.AppointmentStatus == 1) // 假設 2 是已預約狀態
                      .Select(a => new BookedSlot
                         {
                              Start = a.AppointmentStart,
                              End = a.AppointmentEnd
                         })
                      .ToList();
        }

        public List<EmployeeVm> GetBeauticiansByCategoryId(int categoryId)
        {
            var beautician = _db.EmployeeSkills
                                .Include(b => b.Employee)
                                .Where(b => b.CategoryID == categoryId)
                                .Select(b => new EmployeeVm
                                {
                                    EmployeeId = b.EmployeeID,
                                    EmployeeNickname = b.Employee.Nickname,
                                })
                                .ToList();

            return beautician;
        }

        public int GetCategoryIdByServiceId(int serviceId)
        {
            var service = _db.Services.AsNoTracking().FirstOrDefault(s => s.ServiceID == serviceId);
            return service.CategoryID;
        }

        public int GetDetailIdByAppointmentId(int appointmentId)
        {
            var appointment = _db.Appointments
                                 .AsNoTracking()
                                 .FirstOrDefault(a => a.AppointmentID == appointmentId);

            return appointment.OrderDetailID;
        }

        public int GetDuration(int orderDetailId)
        {
            var duration = _db.OrderDetails
                              .AsNoTracking()
                              .Where(o => o.OrderDetailID == orderDetailId)
                              .Include(o => o.Service)
                              .Select(o => o.Service.Duration)
                              .FirstOrDefault();

            return duration;
        }

        public int GetServiceIdByDetailId(int orderDetailId)
        {
            var orderDetail = _db.OrderDetails.AsNoTracking().FirstOrDefault(o => o.OrderDetailID == orderDetailId);
            return orderDetail.ServiceID;
        }

        public void UpdateQuantity(int orderDetailId)
        {
            var db = new AppDbContext();

            var quantity = db.OrderDetails
                            .FirstOrDefault(o => o.OrderDetailID == orderDetailId);

            if (quantity != null)
            {
                quantity.UsedQuantity -= 1;
                quantity.RemainingQuantity += 1;
            }

            db.SaveChanges();
        }

        public void UseQuantity(int orderDetailId)
        {
            var db = new AppDbContext();

            var quantity = db.OrderDetails
                            .FirstOrDefault(o => o.OrderDetailID == orderDetailId);

            if (quantity != null)
            {
                quantity.UsedQuantity += 1;
                quantity.RemainingQuantity -= 1;
            }

            db.SaveChanges();
        }

        public int GetOrderId(int orderDetailId)
        {
            var order = _db.OrderDetails.AsNoTracking().FirstOrDefault(o => o.OrderDetailID == orderDetailId);

            return order.OrderID;
        }

        public void SumQuantity(int orderId)
        {
            var db = new AppDbContext();

            var quantity = db.Orders
                            .FirstOrDefault(o => o.OrderID == orderId);


            if (quantity != null)
            {
                quantity.SumRemainingQuantity -= 1;
            }

            db.SaveChanges();
        }

        public void UpdateTotalQuantity(int orderDetailId)
        {
            var db = new AppDbContext();

            var quantity = db.OrderDetails
                            .Include(o => o.Order)
                            .FirstOrDefault(o => o.OrderDetailID == orderDetailId);


            if (quantity != null)
            {
                quantity.Order.SumRemainingQuantity += 1;
            }

            db.SaveChanges();
        }
    }
}