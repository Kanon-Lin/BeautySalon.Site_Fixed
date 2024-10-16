using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace BeautySalon.Backstage.Site.Models.ViewModels
{
    public class EmployeeVm
    {
        public int EmployeeID { get; set; }
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public int Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        //public int RoleGroupID { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string Image { get; set; }

    }
}