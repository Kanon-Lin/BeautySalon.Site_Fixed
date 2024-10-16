using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautySalon.Backstage.Site.Models.ViewModels
{
    public class LoginVm
    {
        [Display(Name = "帳號")]
        [Required]
        public string EmployeeNo { get; set; }

        [Display(Name = "密碼")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        public string Nickname { get; set; }
    }
}