using AutoMapper;
using BeautySalon.FrontEnd.Site.Models.DTO;
using BeautySalon.FrontEnd.Site.Models.EFModels;
using BeautySalon.FrontEnd.Site.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BeautySalon.FrontEnd.Site.Models.Repositories
{
	public class MemberRepository :IMemberRepository
	{
		private readonly AppDbContext _db;
		private readonly IMapper _mapper;


		public MemberRepository(AppDbContext db,IMapper mapper)
		{
			_db = db;
			_mapper = mapper;
		}

        public void Create(RegisterDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var newMember = new Member
            {
                Account = dto.Account,
                Email = dto.Email,
                EncryptedPassword = dto.EncryptedPassword,
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                Gender = dto.Gender,
                ConfirmCode = dto.ConfirmCode,
                IsConfirmed = dto.IsConfirmed,
                RegistrationDate = DateTime.Now,

            };

            _db.Members.Add(newMember);
            _db.SaveChanges();

        }

        public MemberDto Get(int memberId)
		{
			var member = _db.Members
							.AsNoTracking()
							.FirstOrDefault(x => x.MemberID == memberId);

			if (member == null) return null;

			return new MemberDto
			{
				MemberID = member.MemberID,
				Account = member.Account,
				Email = member.Email,
				EncryptedPassword = member.EncryptedPassword,
				Name = member.Name,
				PhoneNumber = member.PhoneNumber,
				ConfirmCode = member.ConfirmCode,
				IsConfirmed = member.IsConfirmed
			};

			//return _mapper.Map<MemberDto>(member);
		}
		public void Active(int memberId)
		{
			var member = _db.Members.FirstOrDefault(x => x.MemberID == memberId);
			if (member == null)
			{
				throw new InvalidOperationException("會員不存在");
			}

			member.IsConfirmed = true;
			member.ConfirmCode = null;

			_db.SaveChanges();
		}
		public bool IsAccountExist(string account)
		{
			if (string.IsNullOrWhiteSpace(account))
			{
				throw new ArgumentException("帳號不可為空", nameof(account));
			}

			// 使用 Any() 方法來檢查是否存在該帳號
			return  _db.Members
                .AsNoTracking()
                .Any(m => m.Account == account);
		}

		public MemberDto Get(string account)
		{
			if (string.IsNullOrWhiteSpace(account))
			{
				return null;
			}

			var member = _db.Members
				.FirstOrDefault(m => m.Account == account);

			if (member == null) return null;

			return new MemberDto
			{
				MemberID = member.MemberID,
				Account = member.Account,
				Email = member.Email,
				EncryptedPassword = member.EncryptedPassword,
				Name = member.Name,
				PhoneNumber = member.PhoneNumber,
				ConfirmCode = member.ConfirmCode,
				IsConfirmed = member.IsConfirmed
			};
		}

		public void Update(MemberDto memberInDb)
		{
			// 查詢要更新的實體
			var existingMember = _db.Members.FirstOrDefault(x => x.MemberID == memberInDb.MemberID);

			if (existingMember == null)
			{
				throw new InvalidOperationException("會員不存在，無法更新");
			}

			// 手動映射 MemberDto 到現有的 Member 實體
			existingMember.Name = memberInDb.Name;
			existingMember.Email = memberInDb.Email;
			existingMember.PhoneNumber = memberInDb.PhoneNumber;

			_db.SaveChanges();
		}
		public void UpdatePassword(int memberId, string newEncryptedPassword)
		{
			var existingMember = _db.Members.Find(memberId);
			if (existingMember == null)
			{
				throw new InvalidOperationException("會員不存在，無法更新密碼");
			}

			existingMember.EncryptedPassword = newEncryptedPassword;
			_db.SaveChanges();
		}

	}
}