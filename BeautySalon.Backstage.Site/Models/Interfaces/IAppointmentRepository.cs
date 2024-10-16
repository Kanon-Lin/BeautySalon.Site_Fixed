using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Backstage.Site.Models.Interfaces
{
    public interface IAppointmentRepository
    {
        void CancelAppointment(int appointmentId);
        int GetDetailIdByAppointmentId(int appointmentId);
        void UpdateQuantity(int orderDetailId);
        void UpdateTotalQuantity(int orderDetailId);

    }


}
