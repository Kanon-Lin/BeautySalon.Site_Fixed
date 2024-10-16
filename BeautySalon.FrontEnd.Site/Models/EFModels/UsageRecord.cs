namespace BeautySalon.FrontEnd.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UsageRecord")]
    public partial class UsageRecord
    {
        public int UsageRecordID { get; set; }

        public int AppointmentID { get; set; }

        public int OrderID { get; set; }

        public int OrderDetailID { get; set; }

        public int MemberID { get; set; }

        public int Status { get; set; }

        public int CalculateQuantity { get; set; }

        public int CalculateRemainingQuantity { get; set; }

        [Column("UsageRecord")]
        public DateTime UsageRecord1 { get; set; }
    }
}
