namespace BeautySalon.FrontEnd.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrderDetail()
        {
            Appointments = new HashSet<Appointment>();
        }

        public int OrderDetailID { get; set; }

        public int OrderID { get; set; }

        public int ServiceID { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Amount { get; set; }

        public decimal DiscountApplied { get; set; }

        public decimal NetAmount { get; set; }

        public int UsedQuantity { get; set; }

        public int RemainingQuantity { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Appointment> Appointments { get; set; }

        public virtual Order Order { get; set; }

        public virtual Service Service { get; set; }
    }
}
