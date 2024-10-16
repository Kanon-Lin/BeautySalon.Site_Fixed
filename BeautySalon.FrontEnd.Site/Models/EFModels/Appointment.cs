namespace BeautySalon.FrontEnd.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Appointment
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

        public virtual Member Member { get; set; }

        public virtual Order Order { get; set; }

        public virtual OrderDetail OrderDetail { get; set; }

        public virtual Service Service { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
