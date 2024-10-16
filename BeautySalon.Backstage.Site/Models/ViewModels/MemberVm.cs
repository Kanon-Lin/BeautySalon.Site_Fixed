using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautySalon.Backstage.Site.Models.ViewModels
{
    public class MemberVm
    {
        public int MemberID { get; set; }

        public string Name { get; set; }

        public int Gender { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Account { get; set; }

        public DateTime RegistrationDate { get; set; }

    }
}