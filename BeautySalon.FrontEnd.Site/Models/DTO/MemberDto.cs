using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautySalon.FrontEnd.Site.Models.DTO
{
	public class MemberDto
	{
		public int MemberID { get; set; }

		public string Name { get; set; }

		public int Gender { get; set; }

		public string PhoneNumber { get; set; }

		public string Email { get; set; }

		public string Account { get; set; }

		public string EncryptedPassword { get; set; }

		public DateTime RegistrationDate { get; set; }

		public bool? IsConfirmed { get; set; }


		public string ConfirmCode { get; set; }
	}
}