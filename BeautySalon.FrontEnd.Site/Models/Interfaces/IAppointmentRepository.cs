using BeautySalon.FrontEnd.Site.Models.DTO;
using BeautySalon.FrontEnd.Site.Models.EFModels;
using BeautySalon.FrontEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.FrontEnd.Site.Models.Interfaces
{
    public interface IAppointmentRepository
    {
        void CancelAppointment(int appointmentId);
        void CreateAppointment(AppointmentDto dto);
        List<Appointment> GetAppointment(int beauticianId, DateTime date);
        List<EmployeeVm> GetBeauticiansByCategoryId(int categoryId);
        List<BookedSlot> GetBookedTimeSlots(int beauticianId, DateTime date);
        int GetCategoryIdByServiceId(int serviceId);
        int GetDetailIdByAppointmentId(int appointmentId);
        int GetDuration(int serviceId);
        int GetOrderId(int orderDetailId);
        int GetServiceIdByDetailId(int orderDetailId);
        void SumQuantity(int orderId);
        void UpdateQuantity(int orderDetailId);
        void UpdateTotalQuantity(int orderDetailId);
        void UseQuantity(int orderDetailId);
    }
}
