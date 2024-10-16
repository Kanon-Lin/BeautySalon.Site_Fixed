namespace BeautySalon.Backstage.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CustomerInquiry
    {
        [Key]
        public int InquiryID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int Gender { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(10)]
        public string TelephoneNumber { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime InquiryDate { get; set; }
    }
}
