using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySalon.FrontEnd.Site.Models.DTO
{
    public class AppointmentDto
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
}