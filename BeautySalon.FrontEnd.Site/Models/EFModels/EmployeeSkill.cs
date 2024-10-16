namespace BeautySalon.FrontEnd.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EmployeeSkill
    {
        [Key]
        public int EmployeeSkillsID { get; set; }

        public int EmployeeID { get; set; }

        public int CategoryID { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual ServiceCategory ServiceCategory { get; set; }
    }
}
