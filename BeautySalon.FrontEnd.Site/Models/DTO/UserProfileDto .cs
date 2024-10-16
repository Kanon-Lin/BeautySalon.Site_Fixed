using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautySalon.FrontEnd.Site.Models.DTO
{
	public class UserProfileDto
	{
		public int MemberID { get; set; }

		public string Name { get; set; }

		public string PhoneNumber { get; set; }

		public string Email { get; set; }
	}
}