using BeautySalon.FrontEnd.Site.Models.DTO;
using BeautySalon.FrontEnd.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.FrontEnd.Site.Models.Interfaces
{
	public interface IMemberRepository
	{
		void Active(int memberId);
		void Create(RegisterDto dto);
		MemberDto Get(int memberId);
		MemberDto Get(string account);
		bool IsAccountExist(string account);
		void Update(MemberDto memberInDb);
		void UpdatePassword(int memberId, string newEncryptedPassword);
	}
}
