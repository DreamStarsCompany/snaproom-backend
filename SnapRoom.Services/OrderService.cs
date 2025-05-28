using AutoMapper;
using SnapRoom.Contract.Repositories.IUOW;
using SnapRoom.Contract.Services;

namespace SnapRoom.Services
{
	public class OrderService : IOrderService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IAuthService _authenticationService;

		public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authenticationService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_authenticationService = authenticationService;
		}

	}
}
