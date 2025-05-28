using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SnapRoom.Common.Base;
using SnapRoom.Common.Enum;
using SnapRoom.Common.Utils;
using SnapRoom.Contract.Repositories.Dtos.AccountDtos;
using SnapRoom.Contract.Repositories.Entities;
using SnapRoom.Contract.Repositories.IUOW;
using SnapRoom.Contract.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SnapRoom.Services
{
	public class AuthService : IAuthService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IConfiguration _configuration;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
		{
			_unitOfWork = unitOfWork;
			_configuration = configuration;
			_httpContextAccessor = httpContextAccessor;
		}

		public Task ConfirmUpdateEmail(string otp)
		{
			throw new NotImplementedException();
		}

		public Task ForgetPassword(string email)
		{
			throw new NotImplementedException();
		}

		public string GetCurrentAccountId()
		{
			throw new NotImplementedException();
		}

		public string GetCurrentRole()
		{
			throw new NotImplementedException();
		}

		public async Task<string> CustomerLogin(LoginDto loginDto)
		{
			Account? account = await _unitOfWork.GetRepository<Account>().Entities
				.Where(a => a.Email == loginDto.Email && a.DeletedBy == null && a.Role == RoleEnum.Customer)
				.FirstOrDefaultAsync();

			if (account == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, account.Password))
			{
				throw new BaseException.ErrorException(401, "unauthorized", "Sai mật khẩu hoặc tài khoản");
			}

			return GenerateJwtToken(account);
		}

		public async Task<string> DesignerLogin(LoginDto loginDto)
		{
			Account? account = await _unitOfWork.GetRepository<Account>().Entities
				.Where(a => a.Email == loginDto.Email && a.DeletedBy == null && (a.Role == RoleEnum.Designer || a.Role == RoleEnum.Admin))
				.FirstOrDefaultAsync();

			if (account == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, account.Password))
			{
				throw new BaseException.ErrorException(401, "unauthorized", "Sai mật khẩu hoặc tài khoản");
			}

			return GenerateJwtToken(account);
		}

		public async Task Register(RegisterDto registerDto)
		{
			//// Check if the user already exists
			//var existingAccount = await _unitOfWork.GetRepository<Account>().Entities
			//	.Where(a => a.Email == registerDto.Email && a.DeletedBy == null)
			//	.FirstOrDefaultAsync();
			//if (existingAccount != null)
			//{
			//	throw new BaseException.ErrorException(409, "conflict", "Email này đã được sử dụng, vui lòng thử lại");
			//}

			//// Hash the password
			//var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

			//// Create new user entity
			//Account newCustomer = new()
			//{
			//	Name = registerDto.Name,
			//	Password = hashedPassword,
			//	Email = registerDto.Email,
			//	Role = RoleEnum.Customer,
			//	VerificationToken = Guid.NewGuid().ToString()
			//};

			//newCustomer.CreatedBy = newCustomer.Id;
			//newCustomer.LastUpdatedBy = newCustomer.Id;
			//newCustomer.EmailLastUpdatedTime = newCustomer.CreatedTime;
			//// Save account to the database
			//await _unitOfWork.GetRepository<Account>().InsertAsync(newCustomer);
			//await _unitOfWork.SaveAsync();
		}

		public Task ResetPassword(string token, string newPassword)
		{
			throw new NotImplementedException();
		}

		public void UpdateAudits(BaseEntity entity, bool isCreating, bool isDeleting = false)
		{
			throw new NotImplementedException();
		}

		public Task UpdateEmail(string newEmail)
		{
			throw new NotImplementedException();
		}

		public Task UpdatePassword(string password, string newPassword)
		{
			throw new NotImplementedException();
		}

		public Task<bool> VerifyAccount(string token)
		{
			throw new NotImplementedException();
		}

		public Task VerifyResetPassowrd(string token)
		{
			throw new NotImplementedException();
		}

		private string GenerateJwtToken(Account account)
		{
			if (account == null)
			{
				throw new ArgumentNullException(nameof(account), "User object cannot be null.");
			}

			// Retrieve the JWT secret from configuration
			var secret = _configuration["JwtSettings:Secret"];
			if (string.IsNullOrEmpty(secret))
			{
				throw new ArgumentNullException("JwtSettings:Secret", "JWT Secret not found in configuration.");
			}

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			// Create claims based on user information, with null checks
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, account.Id),
				new Claim("Id", account.Id),
				new Claim("Name", account.Name),
				new Claim("Email", account.Email ?? ""),
				new Claim("Role", account.Role.ToString())
			};

			// Retrieve the token expiry period from configuration, handle parsing errors
			if (!int.TryParse(_configuration["JwtSettings:ExpiryInDays"], out var expiryInDays))
			{
				expiryInDays = 1; // Default to 1 day if parsing fails or value is not set
			}

			// Create and return the JWT token
			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.UtcNow.AddDays(expiryInDays),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

	}

}
