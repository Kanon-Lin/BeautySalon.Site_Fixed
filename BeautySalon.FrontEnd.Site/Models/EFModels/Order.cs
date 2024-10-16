namespace BeautySalon.FrontEnd.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            Appointments = new HashSet<Appointment>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderID { get; set; }

        public int MemberID { get; set; }

        public int OrderStatus { get; set; }

        public DateTime OrderDate { get; set; }

        public int TotalQuantity { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal TotalNetAmount { get; set; }

        public decimal DiscountedAmount { get; set; }

        public DateTime? CancelDate { get; set; }

        public int SumRemainingQuantity { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Appointment> Appointments { get; set; }

        public virtual Member Member { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
