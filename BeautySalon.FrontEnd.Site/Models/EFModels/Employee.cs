namespace BeautySalon.FrontEnd.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            Appointments = new HashSet<Appointment>();
            EmployeeSkills = new HashSet<EmployeeSkill>();
        }

        public int EmployeeID { get; set; }

        [Required]
        [StringLength(20)]
        public string EmployeeNo { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Nickname { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        public int Gender { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(10)]
        public string Phone { get; set; }

        public int RoleGroupID { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(256)]
        public string Image { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Appointment> Appointments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeSkill> EmployeeSkills { get; set; }
    }
}
