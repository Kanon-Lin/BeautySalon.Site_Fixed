using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeautySalon.Backstage.Site.Models.ViewModels
{
    public class EmployeeUpdateVm
    {
        // 不包含EmployeeNo 和 CreatedDate
        public int EmployeeID { get; set; }
        public string EmployeeNo { get; set; }

        [Required(ErrorMessage = "姓名是必填的")]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "密碼是必填的")]
        [StringLength(50)]
        // [DataType(DataType.Password)]
        public string Password { get; set; }

        public int Gender { get; set; }

        [EmailAddress(ErrorMessage = "請輸入有效的電子郵件地址")]
        [Remote("CheckEmployeeDetails", "Employees", AdditionalFields = "EmployeeID", ErrorMessage = "Email已存在!!")]
        [StringLength(50)]
        [Index(IsUnique = true)] // 確保唯一性
        public string Email { get; set; }

        [Remote("CheckEmployeeDetails", "Employees", AdditionalFields = "EmployeeID", ErrorMessage = "電話號碼已存在!!")]
        [RegularExpression(@"^\d{1,15}$", ErrorMessage = "電話號碼只能包含數字，且長度不超過15位")]
        [StringLength(10)]
        [Index(IsUnique = true)] // 確保唯一性
        public string Phone { get; set; }

        public DateTime? CreatedDate { get; set; }


        [StringLength(256)]
        public string Image { get; set; }

    }
}