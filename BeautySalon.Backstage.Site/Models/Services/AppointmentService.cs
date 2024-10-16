using BeautySalon.Backstage.Site.Models.Interfaces;
using BeautySalon.Backstage.Site.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySalon.Backstage.Site.Models.Services
{
    public class AppointmentService
    {
        private IAppointmentRepository _repo;
        public AppointmentService()
        {
            _repo = new AppointmentRepository();
        }

        public AppointmentService(IAppointmentRepository repo)
        {
            _repo = repo;
        }
        internal void CancelAppointment(int appointmentId)
        {
            _repo.CancelAppointment(appointmentId);
        }
        internal void UpdateQuantity(int orderDetailId)
        {
            _repo.UpdateQuantity(orderDetailId);
            _repo.UpdateTotalQuantity(orderDetailId);
        }

        internal int GetOrderDetailId(int appointmentId)
        {
            var detailId = _repo.GetDetailIdByAppointmentId(appointmentId);

            return detailId;
        }
    }
}