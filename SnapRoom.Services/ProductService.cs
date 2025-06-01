using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SnapRoom.Common.Base;
using SnapRoom.Contract.Repositories.Entities;
using SnapRoom.Contract.Repositories.IUOW;
using SnapRoom.Contract.Services;

namespace SnapRoom.Services
{
	public class ProductService : IProductService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IAuthService _authenticationService;

		public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authenticationService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_authenticationService = authenticationService;
		}

		public async Task<BasePaginatedList<object>> GetDesigns(int pageNumber, int pageSize)
		{

			List<Product> query = await _unitOfWork.GetRepository<Product>().Entities
				.Where(p => p.Design != null).ToListAsync();

			var responseItems = query.Select(x => new {
				x.Id,
				x.Name,
				x.Rating,
				x.Price,
				Image = new { x.Images?.FirstOrDefault()?.ImageSource },
			}).ToList();

			return new BasePaginatedList<object>(responseItems, query.Count, pageNumber, pageSize);
		}

		public async Task<BasePaginatedList<object>> GetFurnitures(int pageNumber, int pageSize)
		{

			List<Product> query = await _unitOfWork.GetRepository<Product>().Entities
				.Where(p => p.Furniture != null).ToListAsync();

			var responseItems = query.Select(x => new {
				x.Id,
				x.Name,
				x.Rating,
				x.Price,
				Image = new { x.Images?.FirstOrDefault()?.ImageSource },
			}).ToList();

			return new BasePaginatedList<object>(responseItems, query.Count, pageNumber, pageSize);
		}

	}
}
