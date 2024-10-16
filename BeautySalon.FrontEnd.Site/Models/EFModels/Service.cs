namespace BeautySalon.FrontEnd.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Service
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Service()
        {
            Appointments = new HashSet<Appointment>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ServiceID { get; set; }

        [Required]
        [StringLength(100)]
        public string ServiceName { get; set; }

        public int CategoryID { get; set; }

        public decimal Price { get; set; }

        public int Duration { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Appointment> Appointments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual ServiceCategory ServiceCategory { get; set; }
    }
}
