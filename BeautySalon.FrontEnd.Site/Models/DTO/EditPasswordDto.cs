using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautySalon.FrontEnd.Site.Models.DTO
{
	public class EditPasswordDto
	{
		public int MemberId { get; set; }

		[Required(ErrorMessage = "當前密碼不能為空")]
		public string CurrentPassword { get; set; }

		[Required(ErrorMessage = "新密碼不能為空")]
		[MinLength(6, ErrorMessage = "新密碼長度不能少於6個字符")]
		public string Password { get; set; }

		[Compare("Password", ErrorMessage = "確認密碼與新密碼不匹配")]
		public string ConfirmPassword { get; set; }
	}
}