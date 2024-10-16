using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySalon.Backstage.Site.Models.ViewModels
{
    public class AppointmentVm
    {
        public int AppointmentID { get; set; }

        public int MemberID { get; set; }

        public int OrderID { get; set; }

        public int OrderDetailID { get; set; }

        public int ServiceID { get; set; }

        public int? EmployeeID { get; set; }

        public DateTime AppointmentStart { get; set; }

        public DateTime AppointmentEnd { get; set; }

        public int AppointmentStatus { get; set; }


        public string ServiceName { get; set; }//服務項目名稱
        public string Nickname { get; set; }//員工暱稱
        public string Name { get; set; }//會員姓名
    }
}