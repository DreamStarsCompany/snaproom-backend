using Microsoft.AspNetCore.Mvc;
using SnapRoom.Contract.Services;

namespace SnapRoom.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountsController : ControllerBase
	{
		private readonly IAccountService _accountService;

		public AccountsController(IAccountService accountService)
		{
			_accountService = accountService;
		}

	}
}
