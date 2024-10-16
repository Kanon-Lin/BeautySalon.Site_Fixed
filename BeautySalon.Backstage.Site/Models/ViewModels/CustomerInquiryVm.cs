using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySalon.Backstage.Site.Models.ViewModels
{
    public class CustomerInquiryVm
    {
        public int InquiryID { get; set; }

        public string Name { get; set; }

        public int Gender { get; set; }

        public string Email { get; set; }

        public string TelephoneNumber { get; set; }

        public string Message { get; set; }

        public DateTime InquiryDate { get; set; }
    }
}