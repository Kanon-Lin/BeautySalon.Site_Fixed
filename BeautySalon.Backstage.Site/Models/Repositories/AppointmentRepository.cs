using BeautySalon.Backstage.Site.Models.EFModels;
using BeautySalon.Backstage.Site.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BeautySalon.Backstage.Site.Models.Repositories
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

        public int GetDetailIdByAppointmentId(int appointmentId)
        {
            var appointment = _db.Appointments
                                .AsNoTracking()
                                .FirstOrDefault(a => a.AppointmentID == appointmentId);

            return appointment.OrderDetailID;
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