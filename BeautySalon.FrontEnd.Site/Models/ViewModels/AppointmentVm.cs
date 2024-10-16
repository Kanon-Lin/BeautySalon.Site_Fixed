using BeautySalon.FrontEnd.Site.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySalon.FrontEnd.Site.Models.ViewModels
{
    public class AppointmentVm
    {
        public int AppointmentDetailId { get; set; }
        public int MemberId { get; set; }
        public int OrderId { get; set; }
        public int OrderDetailId { get; set; }
        public string ServiceName { get; set; }
        public int ServiceId { get; set; }
        public string Beautician { get; set; }
        public int BeauticianId { get; set; }
        public string Date { get; set; }
        public string TimeSlot { get; set; }
        public int Duration { get; set; }
        public int Status { get; set; }
    }

    public static class AppointmentExt
    {
        public static AppointmentDto ToDto(this AppointmentVm vm)
        {
            return new AppointmentDto
            {
                AppointmentDetailId = vm.AppointmentDetailId,
                MemberId = vm.MemberId,
                OrderId = vm.OrderId,
                OrderDetailId = vm.OrderDetailId,
                ServiceName = vm.ServiceName,
                ServiceId = vm.ServiceId,
                Beautician = vm.Beautician,
                BeauticianId = vm.BeauticianId,
                Date = vm.Date,
                TimeSlot = vm.TimeSlot,
                Duration = vm.Duration,
                Status = vm.Status,
            };
        }
    }
}