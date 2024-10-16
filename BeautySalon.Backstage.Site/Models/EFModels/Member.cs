namespace BeautySalon.Backstage.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Member
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Member()
        {
            Appointments = new HashSet<Appointment>();
            Orders = new HashSet<Order>();
        }

        public int MemberID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int Gender { get; set; }

        [Required]
        [StringLength(10)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string Account { get; set; }

        [StringLength(100)]
        public string EncryptedPassword { get; set; }

        public DateTime RegistrationDate { get; set; }

        public bool? IsConfirmed { get; set; }

        [StringLength(50)]
        public string ConfirmCode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Appointment> Appointments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
