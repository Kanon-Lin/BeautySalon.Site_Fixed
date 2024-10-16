using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using AutoMapper;
using BeautySalon.FrontEnd.Site.Models;
using BeautySalon.FrontEnd.Site.Models.DTO;
using BeautySalon.FrontEnd.Site.Models.EFModels;
using BeautySalon.FrontEnd.Site.Models.Repositories;
using BeautySalon.FrontEnd.Site.Models.Services;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using BeautySalon.FrontEnd.Site.Models.Infra;
using System.Linq;


namespace BeautySalon.FrontEnd.Site.Controllers.APIs
{
    [RoutePrefix("api/MembersApi")]
    public class MembersApiController : ApiController
    {
        private readonly MemberService _memberService;

        public MembersApiController()
        {
            // 在構造函數中初始化 MemberService
            var dbContext = new AppDbContext(); // 確保這裡使用了正確的連接字符串
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<MemberDto, Member>();
                // 添加其他需要的映射
            });
            var mapper = config.CreateMapper();
            var emailService = new EmailService(); // 實例化 EmailService

            var memberRepository = new MemberRepository(dbContext, mapper);
            _memberService = new MemberService(memberRepository, emailService);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IHttpActionResult> Register([FromBody] RegisterDto dto)
        {

            if (dto == null || string.IsNullOrWhiteSpace(dto.Account) ||
                string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("請填寫所有必填欄位");
            }

            if (!IsValidEmail(dto.Email))
            {
                return BadRequest("電子郵件格式不正確");
            }

            try
            {
                // 调用 Service 的 Register 方法
                await _memberService.Register(dto, Request);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "註冊成功，請檢查您的電子郵件來進行驗證。"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("註冊失敗：" + ex.Message);
            }

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ActiveRegister")]
        public IHttpActionResult ActiveRegister([FromBody] ActiveRegisterDto dto)
        {

            try
            {
                _memberService.ActiveRegister(dto.memberId, dto.confirmCode);
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "會員已成功啟用"
                });
            }
            catch (Exception ex)
            {
                // Log exception for debugging
                System.Diagnostics.Debug.WriteLine($"Activation error: {ex.Message}");
                return BadRequest("啟用失敗：" + ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login(LoginDto dto)
        {
            if (string.IsNullOrEmpty(dto.Account) || string.IsNullOrEmpty(dto.Password))
            {
                return BadRequest("帳號或密碼不能為空");
            }

            try
            {
                var (result, token, memberName) = _memberService.ValidateLogin(dto);
                if (result.IsSuccess)
                {
                    return Ok(new
                    {
                        success = true,
                        token = token,
                        memberName = memberName
                    });
                }
                return BadRequest(result.ErrorMessage);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email && Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            }
            catch
            {
                return false;
            }
        }

		[Authorize]
		[HttpGet]
		[Route("Profile")]
		public IHttpActionResult GetProfile()
		{
			try
			{
				if (!User.Identity.IsAuthenticated)
				{
					return Unauthorized();
				}

				var userIdClaim = User.Identity as ClaimsIdentity;
				var memberIdString = userIdClaim?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

				if (string.IsNullOrEmpty(memberIdString) || !int.TryParse(memberIdString, out int memberId))
				{
					// 記錄所有的聲明，以便調試
					var claims = User.Identity as ClaimsIdentity;
					foreach (var claim in claims.Claims)
					{
						System.Diagnostics.Debug.WriteLine($"Claim: {claim.Type} = {claim.Value}");
					}

					return BadRequest("Unable to retrieve valid member ID");
				}

				var profile = _memberService.GetMemberProfile(memberId);
				if (profile == null)
				{
					return NotFound();
				}
				return Ok(profile);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error in GetProfile: {ex.Message}");
				return InternalServerError(ex);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Profile")]
        public IHttpActionResult UpdateProfile([FromBody] EditProfileDto dto)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            try
            {
                int memberId = User.Identity.GetUserId<int>();
                dto.MemberID = memberId;
                Result result = _memberService.UpdateMemberProfile(memberId, dto);
                if (result.IsSuccess)
                {
                    return Ok(new { Success = true, Message = "個人資料更新成功" });
                }
                return BadRequest(result.ErrorMessage);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

		[Authorize]
		[HttpPost]
		[Route("Password")]
		public IHttpActionResult ChangePassword([FromBody] EditPasswordDto dto)
		{
			if (dto == null)
			{
				return Content(HttpStatusCode.BadRequest, new { Success = false, Message = "請提供密碼更新資料" });
			}

			if (!ModelState.IsValid)
			{
				var errors = ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
					.ToList();
				return Content(HttpStatusCode.BadRequest, new { Success = false, Message = "無效的輸入", Errors = errors });
			}

			try
			{
				var userIdClaim = User.Identity as ClaimsIdentity;
				var memberIdString = userIdClaim?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

				if (string.IsNullOrEmpty(memberIdString) || !int.TryParse(memberIdString, out int memberId))
				{
					return Content(HttpStatusCode.BadRequest, new { Success = false, Message = "無法獲取有效的會員ID" });
				}

				dto.MemberId = memberId;
				Result result = _memberService.ChangePassword(dto);

				if (result.IsSuccess)
				{
					return Ok(new { Success = true, Message = "密碼已成功更新" });
				}
				return Content(HttpStatusCode.BadRequest, new { Success = false, Message = result.ErrorMessage });
			}
			catch (Exception ex)
			{
				// 在實際應用中，應該記錄異常
				return InternalServerError(new Exception("更改密碼時發生錯誤，請稍後再試"));
			}
		}

	}

    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string RedirectUrl { get; set; }
    }
}