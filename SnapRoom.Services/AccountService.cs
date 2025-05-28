using AutoMapper;
using Microsoft.Extensions.Configuration;
using SnapRoom.Contract.Repositories.IUOW;
using SnapRoom.Contract.Services;

namespace SnapRoom.Services
{
	public class AccountService : IAccountService
	{

		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IAuthService _authenticationService;
		private readonly IConfiguration _config;

		public AccountService(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authenticationService, IConfiguration config)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_authenticationService = authenticationService;
			_config = config;
		}
	}
}
