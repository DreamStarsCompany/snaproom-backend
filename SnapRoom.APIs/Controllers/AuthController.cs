using Microsoft.AspNetCore.Mvc;
using SnapRoom.Common.Base;
using SnapRoom.Common.Enum;
using SnapRoom.Contract.Repositories.Dtos.AccountDtos;
using SnapRoom.Contract.Services;

namespace SnapRoom.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("customer/login")]
		public async Task<IActionResult> CustomerLogin(LoginDto loginDto)
		{
			string token = await _authService.CustomerLogin(loginDto);

			return Ok(new BaseResponse<object>(
				statusCode: StatusCodeEnum.OK,
				message: "Đăng nhập thành công",
				data: token
			));
		}

		[HttpPost("designer/login")]
		public async Task<IActionResult> DesignerLogin(LoginDto loginDto)
		{
			string token = await _authService.DesignerLogin(loginDto);

			return Ok(new BaseResponse<object>(
				statusCode: StatusCodeEnum.OK,
				message: "Đăng nhập thành công",
				data: token
			));
		}

		[HttpPost("customer/register")]
		public async Task<IActionResult> CustomerRegister(RegisterDto registerDto)
		{
			await _authService.CustomerRegister(registerDto);

			return Ok(new BaseResponse<object>(
				statusCode: StatusCodeEnum.OK,
				message: "Đăng ký thành công",
				data: null
			));
		}

		[HttpPost("designer/register")]
		public async Task<IActionResult> DesignerRegister(RegisterDto registerDto)
		{
			await _authService.DesignerRegister(registerDto);

			return Ok(new BaseResponse<object>(
				statusCode: StatusCodeEnum.OK,
				message: "Đăng ký thành công",
				data: null
			));
		}

		[HttpGet("verify-account")]
		public async Task<IActionResult> VerifyAccount(string token)
		{
			await _authService.VerifyAccount(token);

			return Redirect("https://www.google.com/");
		}


	}
}
