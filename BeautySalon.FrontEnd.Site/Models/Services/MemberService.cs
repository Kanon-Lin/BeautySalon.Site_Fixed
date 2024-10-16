using BeautySalon.FrontEnd.Site.Models.DTO;
using BeautySalon.FrontEnd.Site.Models.EFModels;
using BeautySalon.FrontEnd.Site.Models.Infra;
using BeautySalon.FrontEnd.Site.Models.Interfaces;
using BeautySalon.FrontEnd.Site.Models.Repositories;
using BeautySalon.FrontEnd.Site.Models.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using AutoMapper;
using System.Net.Http;
using System.Threading.Tasks;
using System.Data.Entity.Validation;


namespace BeautySalon.FrontEnd.Site.Models.Services
{
	public class MemberService
	{
		private IMemberRepository _repo;
		private EmailService _emailService; // 注入 EmailService
		public MemberService(IMemberRepository repo, EmailService emailService)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
		}

        public async Task Register(RegisterDto dto, HttpRequestMessage request)
        {


            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "會員資料不可為空");
            }

            //在此可以叫用 EF 或 SERVICE OBJECT 進行新增會員
            //判斷帳號是否已存在
            bool IsAccountExist = _repo.IsAccountExist(dto.Account);
            if (IsAccountExist) throw new Exception("帳號已存在");


            //建立新會員
            //雜湊
            string salt = HashUtility.GetSalt();           //取得Salt
            dto.EncryptedPassword = HashUtility.ToSHA256(dto.Password, salt);

            Console.WriteLine($"EncryptedPassword: {dto.EncryptedPassword}");


            //(更新 DTO 的相關屬性)
            dto.ConfirmCode = Guid.NewGuid().ToString("N");
            dto.IsConfirmed = false;

            _repo.Create(dto);

            // 获取新创建的会员信息
            var newMember = _repo.Get(dto.Account);
            if (newMember == null)
            {
                throw new Exception("無法獲取新創建的會員信息");
            }

            // 生成驗證連結
            var confirmationLink = GenerateConfirmationLink(newMember.MemberID, "ActiveRegister", newMember.ConfirmCode, request);

            // 發送驗證信件
            await _emailService.SendVerificationEmail(dto.Email, confirmationLink);

        }

        private string GenerateConfirmationLink(object memberId, string function, string confirmCode, HttpRequestMessage request)
        {
            var scheme = request.RequestUri.Scheme;
            var host = request.RequestUri.Host;
            var port = request.RequestUri.Port;

            // 組合驗證連結
            string confirmLink = $"{scheme}://{host}:{port}/Html/{function}.html?memberId={memberId}&confirmCode={confirmCode}";
            return confirmLink;
        }

        public void ActiveRegister(int memberId, string confirmCode)
        {
            System.Diagnostics.Debug.WriteLine($"Received: MemberId={memberId}, ConfirmCode={confirmCode}");
            //判斷memberId、confirmCode是否正確，正確就更改會員狀態為啟用
            MemberDto dto = _repo.Get(memberId);
            if (dto == null) throw new Exception("會員不存在");
            if (dto.ConfirmCode != confirmCode) throw new Exception("驗證碼錯誤");
            if (dto.IsConfirmed.HasValue && dto.IsConfirmed.Value) throw new Exception("會員已啟用");

            //啟用會員
            _repo.Active(memberId);
        }
        public (Result result, string token, string memberName) ValidateLogin(LoginDto dto)
		{
			// 判斷帳號是否存在(找出user)
			MemberDto member = _repo.Get(dto.Account);
			if (member == null) return (Result.Fail("帳號不存在"), null, null);

			// 判斷帳號是否已啟用
			if (!member.IsConfirmed.HasValue || member.IsConfirmed.Value == false) return (Result.Fail("帳號未開通"), null, null);

			// 密碼雜湊後比對
			string hashPassword = HashUtility.ToSHA256(dto.Password);
			bool isPasswordCorrect = (hashPassword.CompareTo(member.EncryptedPassword) == 0);
			if (!isPasswordCorrect)
				return (Result.Fail("帳號或密碼錯誤"), null, null); // 密碼錯誤

			// 如果帳號和密碼正確，生成 JWT Token
			string token = GenerateJwtToken(dto.Account, member.MemberID);  // 注意這裡傳入了 MemberID

			// 回傳成功結果、生成的 token 和會員名稱
			return (Result.Success(), token, member.Name);
		}

		// 生成 JWT Token
		private string GenerateJwtToken(string account, int memberId)
		{
			var key = Encoding.ASCII.GetBytes("adfhaertg123485earhbzdfaerasdfgaaerygahnerghnnerhnasdfcvrtujtrn");
			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
			new Claim(ClaimTypes.Name, account),
			new Claim(ClaimTypes.NameIdentifier, memberId.ToString()) // 添加 MemberID
				}),
				Expires = DateTime.UtcNow.AddMonths(6),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public MemberDto GetMemberProfile(int memberId)
		{
			return _repo.Get(memberId);
		}

		public Result UpdateMemberProfile(int memberId,EditProfileDto dto)
		{
			try
			{
				var memberInDb = _repo.Get(dto.MemberID);
				if (memberInDb == null)
				{
					return Result.Fail("會員不存在");
				}
				// 額外的驗證
				if (dto.Name.Length > 50)
				{
					return Result.Fail("姓名長度不能超過50個字符");
				}

				if (dto.Email.Length > 50)
				{
					return Result.Fail("電子郵件長度不能超過50個字符");
				}
				// 檢查 PhoneNumber 是否為 null
				if (string.IsNullOrEmpty(dto.PhoneNumber))
				{
					return Result.Fail("電話號碼不能為空");
				}
				if (dto.PhoneNumber.Length != 10 || !dto.PhoneNumber.All(char.IsDigit))
				{
					return Result.Fail("電話號碼必須為10位數字");
				}

				// 更新會員資料
				memberInDb.Name = dto.Name;
				memberInDb.PhoneNumber = dto.PhoneNumber;
				memberInDb.Email = dto.Email;

				_repo.Update(memberInDb);
				return Result.Success();
			}
			catch (DbEntityValidationException ex)
			{
				foreach (var validationErrors in ex.EntityValidationErrors)
				{
					foreach (var validationError in validationErrors.ValidationErrors)
					{
						System.Diagnostics.Debug.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
					}
				}

				var errorMessages = ex.EntityValidationErrors
					.SelectMany(x => x.ValidationErrors)
					.Select(x => $"{x.PropertyName}: {x.ErrorMessage}");

				var fullErrorMessage = string.Join("; ", errorMessages);
				var exceptionMessage = string.Concat(ex.Message, " 驗證錯誤: ", fullErrorMessage);

				return Result.Fail("更新失敗: " + exceptionMessage);
			}


		}

		public Result ChangePassword(EditPasswordDto dto)
		{
			var member = _repo.Get(dto.MemberId);
			if (member == null)
			{
				return Result.Fail("會員不存在");
			}

			// 驗證當前密碼
			string currentPasswordHash = HashUtility.ToSHA256(dto.CurrentPassword);
			if (currentPasswordHash != member.EncryptedPassword)
			{
				return Result.Fail("當前密碼不正確");
			}

			// 更新密碼
			string salt = HashUtility.GetSalt();
			string newPasswordHash = HashUtility.ToSHA256(dto.Password, salt);

			// 使用新的 UpdatePassword 方法
			_repo.UpdatePassword(dto.MemberId, newPasswordHash);

			return Result.Success();
		}


	}
}