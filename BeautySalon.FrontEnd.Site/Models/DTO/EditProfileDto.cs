using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautySalon.FrontEnd.Site.Models.DTO
{
	public class EditProfileDto
	{
		public int MemberID { get; set; }

		[Required(ErrorMessage = "姓名不能為空")]
		[StringLength(50, ErrorMessage = "姓名長度不能超過50個字符")]
		public string Name { get; set; }

		[Required(ErrorMessage = "電子郵件不能為空")]
		[EmailAddress(ErrorMessage = "請輸入有效的電子郵件地址")]
		[StringLength(50, ErrorMessage = "電子郵件長度不能超過50個字符")]
		public string Email { get; set; }

		[Required(ErrorMessage = "電話號碼不能為空")]
		[StringLength(10, ErrorMessage = "電話號碼長度必須為10個字符")]
		[RegularExpression(@"^\d{10}$", ErrorMessage = "請輸入10位數字的電話號碼")]
		public string PhoneNumber { get; set; }
	}
}