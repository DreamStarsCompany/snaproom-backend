using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SnapRoom.Common.Base;
using SnapRoom.Common.Enum;
using SnapRoom.Common.Utils;
using SnapRoom.Contract.Repositories.Dtos.ProductDtos;
using SnapRoom.Contract.Repositories.Entities;
using SnapRoom.Contract.Repositories.IUOW;
using SnapRoom.Contract.Services;
using static SnapRoom.Common.Base.BaseException;

namespace SnapRoom.Services
{
	public class ProductService : IProductService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IAuthService _authService;

		public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authenticationService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_authService = authenticationService;
		}

		public async Task<BasePaginatedList<object>> GetDesigns(int pageNumber, int pageSize)
		{

			List<Product> query = await _unitOfWork.GetRepository<Product>().Entities
				.Where(p => p.Design != null).ToListAsync();

			var responseItems = query.Select(x => new
			{
				x.Id,
				x.Name,
				x.Description,
				x.Rating,
				x.Price,
				PrimaryImage = new { x.Images?.FirstOrDefault(img => img.IsPrimary)?.ImageSource },
				Categories = x.ProductCategories?.Select(pc => new
				{
					Id = pc.CategoryId,
					Name = _unitOfWork.GetRepository<Category>().Entities
						.Where(c => c.Id == pc.CategoryId)
						.Select(c => c.Name).FirstOrDefault()
				}).ToList(),
			}).ToList();

			return new BasePaginatedList<object>(responseItems, query.Count, pageNumber, pageSize);
		}

		public async Task<BasePaginatedList<object>> GetDesignsForDesigner(int pageNumber, int pageSize)
		{
			string designerId = _authService.GetCurrentAccountId();

			Account? designer = await _unitOfWork.GetRepository<Account>().Entities.Where(a => a.Id == designerId && a.Role == RoleEnum.Designer && a.DeletedBy == null).FirstOrDefaultAsync();

			if (designer == null)
			{
				throw new ErrorException(404, "", "Tài khoản không hợp lệ");
			}

			List<Product> query = await _unitOfWork.GetRepository<Product>().Entities
				.Where(p => p.Design != null && p.DesignerId == designerId).ToListAsync();

			var responseItems = query.Select(x => new
			{
				x.Id,
				x.Name,
				x.Description,
				x.Rating,
				x.Price,
				PrimaryImage = new { x.Images?.FirstOrDefault(img => img.IsPrimary)?.ImageSource },
				Categories = x.ProductCategories?.Select(pc => new
				{
					Id = pc.CategoryId,
					Name = _unitOfWork.GetRepository<Category>().Entities
						.Where(c => c.Id == pc.CategoryId)
						.Select(c => c.Name).FirstOrDefault()
				}).ToList(),
			}).ToList();

			return new BasePaginatedList<object>(responseItems, query.Count, pageNumber, pageSize);
		}


		public async Task<BasePaginatedList<object>> GetFurnitures(int pageNumber, int pageSize)
		{

			List<Product> query = await _unitOfWork.GetRepository<Product>().Entities
				.Where(p => p.Furniture != null).ToListAsync();

			var responseItems = query.Select(x => new
			{
				x.Id,
				x.Name,
				x.Description,
				x.Rating,
				x.Price,
				PrimaryImage = new { x.Images?.FirstOrDefault(img => img.IsPrimary)?.ImageSource },
				Categories = x.ProductCategories?.Select(pc => new
				{
					Id = pc.CategoryId,
					Name = _unitOfWork.GetRepository<Category>().Entities
						.Where(c => c.Id == pc.CategoryId)
						.Select(c => c.Name).FirstOrDefault()
				}).ToList(),

			}).ToList();

			return new BasePaginatedList<object>(responseItems, query.Count, pageNumber, pageSize);
		}

		public async Task<BasePaginatedList<object>> GetFurnituresForDesigner(int pageNumber, int pageSize)
		{

			string designerId = _authService.GetCurrentAccountId();

			Account? designer = await _unitOfWork.GetRepository<Account>().Entities.Where(a => a.Id == designerId && a.Role == RoleEnum.Designer && a.DeletedBy == null).FirstOrDefaultAsync();

			if (designer == null)
			{
				throw new ErrorException(404, "", "Tài khoản không hợp lệ");
			}

			List<Product> query = await _unitOfWork.GetRepository<Product>().Entities
				.Where(p => p.Furniture != null && p.DesignerId == designerId).ToListAsync();
			 

			var responseItems = query.Select(x => new
			{
				x.Id,
				x.Name,
				x.Description,
				x.Rating,
				x.Price,
				PrimaryImage = new { x.Images?.FirstOrDefault(img => img.IsPrimary)?.ImageSource },
				Categories = x.ProductCategories?.Select(pc => new
				{
					Id = pc.CategoryId,
					Name = _unitOfWork.GetRepository<Category>().Entities
						.Where(c => c.Id == pc.CategoryId)
						.Select(c => c.Name).FirstOrDefault()
				}).ToList(),

			}).ToList();

			return new BasePaginatedList<object>(responseItems, query.Count, pageNumber, pageSize);
		}


		public async Task CreateDesign(DesignCreateDto dto)
		{
			string designerId = _authService.GetCurrentAccountId();

			Account? designer = await _unitOfWork.GetRepository<Account>().Entities
				.Where(a => a.Id == designerId && a.Role == RoleEnum.Designer)
				.FirstOrDefaultAsync();

			if (designer == null)
			{
				throw new ErrorException(404, "", "Tài khoản không hợp lệ");
			}

			Category? style = await _unitOfWork.GetRepository<Category>().Entities
				.Where(c => c.Id == dto.StyleId && c.Style && c.DeletedBy == null)
				.FirstOrDefaultAsync();

			// Initialize the design product
			Product design = new()
			{
				Id = Guid.NewGuid().ToString(), // Ensure a unique ID before using it in related entities
				Name = dto.Name,
				Description = dto.Description,
				DesignerId = designerId,
				Price = dto.Price,
				Active = dto.Active,
				Rating = 0.0,
				ProductCategories = new List<ProductCategory>()
			};

			// Attach the design entity
			design.Design = new Design
			{
				Id = design.Id
			};

			design.ProductCategories.Add(new ProductCategory
			{
				ProductId = design.Id,
				CategoryId = dto.StyleId
			});

			// Prepare categories
			foreach (var categoryId in dto.CategoryIds.Distinct()) // prevent duplicates just in case
			{
				var categoryExists = await _unitOfWork.GetRepository<Category>().Entities
					.AnyAsync(c => c.Id == categoryId && !c.Style && c.DeletedBy == null);

				if (!categoryExists)
				{
					throw new ErrorException(404, "", "Danh mục không hợp lệ");
				}

				design.ProductCategories.Add(new ProductCategory
				{
					ProductId = design.Id,
					CategoryId = categoryId
				});
			}

			await _unitOfWork.GetRepository<Product>().InsertAsync(design);
			await _unitOfWork.SaveAsync();
		}
	}
}
