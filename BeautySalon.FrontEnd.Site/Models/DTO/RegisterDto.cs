using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautySalon.FrontEnd.Site.Models.DTO
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "帳號是必填欄位")]
        [StringLength(20, ErrorMessage = "帳號長度不能超過 20 個字元")] // 調整長度限制
        public string Account { get; set; }

        [Required(ErrorMessage = "密碼是必填欄位")]
        [StringLength(100, ErrorMessage = "密碼長度不能超過 100 個字元")] // 與 EncryptedPassword 長度一致
        public string Password { get; set; }

        // 電子郵件，必填且格式需正確
        [Required(ErrorMessage = "電子郵件是必填欄位")]
        [EmailAddress(ErrorMessage = "電子郵件格式不正確")]
        [StringLength(50, ErrorMessage = "電子郵件長度不能超過 50 個字元")]
        public string Email { get; set; }

        [Required(ErrorMessage = "姓名是必填欄位")]
        [StringLength(50, ErrorMessage = "姓名長度不能超過 50 個字元")]
        public string Name { get; set; }

        // 性別，應該驗證是否符合系統支持的值
        [Range(0, 2, ErrorMessage = "性別值不正確")]
        public int Gender { get; set; }

        // 電話號碼，選填或必填，長度限制為 10 字元
        [Phone(ErrorMessage = "電話號碼格式不正確")]
        [StringLength(10, ErrorMessage = "電話號碼長度不能超過 10 個字元")]
        public string PhoneNumber { get; set; }


        // 以下屬性在後端設置，前端不需要提供
        public string EncryptedPassword { get; set; }
        public string ConfirmCode { get; set; }
        public bool IsConfirmed { get; set; } = false;
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}