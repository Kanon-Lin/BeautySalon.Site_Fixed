using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeautySalon.Backstage.Site.Models.ViewModels
{
    public class EmployeeCreateVm
    {
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "員工編號是必填的")]
        [StringLength(20)]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "員工編號只能包含英文和數字")]
        [Remote("CheckEmployeeNo", "Employees", ErrorMessage = "員工編號已存在!!")]
        [MinLength(2, ErrorMessage = "員工編號至少需要6個字符")]
        [MaxLength(10, ErrorMessage = "員工編號不能超過10個字符")]
        [Index(IsUnique = true)] // 確保唯一性
        public string EmployeeNo { get; set; }

        [Required(ErrorMessage = "姓名是必填的")]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "密碼是必填的")]
        [StringLength(50)]
        //[DataType(DataType.Password)]
        public string Password { get; set; }

        public int Gender { get; set; }

        [EmailAddress(ErrorMessage = "請輸入有效的電子郵件地址")]
        [Remote("CheckEmployeeDetails", "Employees", ErrorMessage = "Email已存在!!")]
        [StringLength(50)]
        [Index(IsUnique = true)] // 確保唯一性
        public string Email { get; set; }

        [RegularExpression(@"^\d{1,15}$", ErrorMessage = "電話號碼只能包含數字，且長度不超過15位")]
        [StringLength(10)]
        [Remote("CheckEmployeeDetails", "Employees", ErrorMessage = "電話號碼已存在!!")]
        [Index(IsUnique = true)] // 確保唯一性
        public string Phone { get; set; }

        public int RoleGroupID { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(256)]
        public string Image { get; set; }
    }
}