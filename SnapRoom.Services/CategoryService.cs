using AutoMapper;
using SnapRoom.Contract.Repositories.IUOW;
using SnapRoom.Contract.Services;

namespace SnapRoom.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IAuthService _authenticationService;

		public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authenticationService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_authenticationService = authenticationService;
		}
	}
}
