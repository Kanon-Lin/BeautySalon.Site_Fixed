using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautySalon.FrontEnd.Site.Models.ViewModels
{
    public class CustomerInquiryVm
    {
        public int InquiryID { get; set; }

        [Required(ErrorMessage = "請輸入您的姓名")]
        [StringLength(100, ErrorMessage = "姓名不能超過 100 字元")]
        public string Name { get; set; }

        [Required(ErrorMessage = "請選擇性別")]
        public int Gender { get; set; } // 1: 男, 2: 女

        [Required(ErrorMessage = "請輸入您的電子郵件")]
        [StringLength(100, ErrorMessage = "Email 不能超過 100 字元")]
        [EmailAddress(ErrorMessage = "無效的電子郵件格式")]
        public string Email { get; set; }

        [Required(ErrorMessage = "請輸入您的電話號碼")]
        [StringLength(10, ErrorMessage = "電話號碼不能超過 10 位數")]
        [Phone(ErrorMessage = "無效的電話號碼格式")]
        public string TelephoneNumber { get; set; }

        [Required(ErrorMessage = "請輸入您的留言")]
        public string Message { get; set; }

        public DateTime InquiryDate { get; set; } = DateTime.Now;
    }
}