using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySalon.Backstage.Site.Models.ViewModels
{
    public class HomePageVm
    {
        public IEnumerable<OrderVm> Orders { get; set; }
        public IEnumerable<AppointmentVm> Appointments { get; set; }


        public DateTime SelectedDate { get; set; } // 新增屬性
    }
}