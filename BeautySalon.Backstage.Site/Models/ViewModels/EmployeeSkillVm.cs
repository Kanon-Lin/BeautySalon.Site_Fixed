using BeautySalon.Backstage.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySalon.Backstage.Site.Models.ViewModels
{
    public class EmployeeSkillVm
    {
        public int EmployeeSkillsID { get; set; }

        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }

        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        public bool IsSelected { get; set; }

        public List<EmployeeSkill> EmployeeSkills { get; set; }

        public EmployeeSkillVm()
        {
            EmployeeSkills = new List<EmployeeSkill>();
        }
    }
}