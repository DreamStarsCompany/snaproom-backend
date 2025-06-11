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

			if (style == null)
			{
				throw new ErrorException(404, "", "Danh mục phong cách không hợp lệ");
			}

			Product design = new()
			{
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

		public async Task CreateFurniture(FurnitureCreateDto dto)
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

			if (style == null)
			{
				throw new ErrorException(404, "", "Danh mục phong cách không hợp lệ");
			}

			Product furniture = new()
			{
				Name = dto.Name,
				Description = dto.Description,
				ParentDesignId = dto.ParentDesignId,
				DesignerId = designerId,
				Price = dto.Price,
				Active = dto.Active,
				Rating = 0.0,
				ProductCategories = new List<ProductCategory>()
			};

			// Attach the design entity
			furniture.Furniture = new Furniture
			{
				Id = furniture.Id
			};

			furniture.ProductCategories.Add(new ProductCategory
			{
				ProductId = furniture.Id,
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

				furniture.ProductCategories.Add(new ProductCategory
				{
					ProductId = furniture.Id,
					CategoryId = categoryId
				});
			}

			await _unitOfWork.GetRepository<Product>().InsertAsync(furniture);
			await _unitOfWork.SaveAsync();
		}


		public async Task<object> GetProductById(string id)
		{

			Product? product = await _unitOfWork.GetRepository<Product>().Entities
				.Where(p => p.Id == id && p.DeletedBy == null).FirstOrDefaultAsync();

			if (product == null)
			{
				throw new ErrorException(404, "", "Mã sản phẩm không hợp lệ");
			}

			if (product.Design != null)
			{
				return await GetDesign(product);
			}
			else if (product.Furniture != null)
			{
				return await GetFurniture(product);
			}

			throw new ErrorException(404, "", "Sản phẩm chưa được phân loại");
		}

		private async Task<object> GetFurniture(Product product)
		{
			await _unitOfWork.SaveAsync();

			var responseItem = new
			{
				product.Id,
				product.Name,
				product.Description,
				product.Rating,
				product.Price,
				product.Active,
				Designer = new { product.Designer?.Id, product.Designer?.Name },
				ParentDesign = new { product.ParentDesign?.Id, product.ParentDesign?.Name },
				PrimaryImage = new { product.Images?.FirstOrDefault(img => img.IsPrimary)?.ImageSource },
				Images = product.Images?
					.Where(img => !img.IsPrimary)
					.Select(img => new { img.ImageSource})
					.ToList(),
				Style = product.ProductCategories?
					.Select(pc => _unitOfWork.GetRepository<Category>().Entities
						.Where(c => c.Id == pc.CategoryId)
						.Select(c => new { c.Id, c.Name, c.Style })
						.FirstOrDefault())
					.Where(c => c != null)
					.OrderByDescending(c => c?.Style)
					.Select(c => new { c?.Id, c?.Name })
					.FirstOrDefault(),
				Categories = product.ProductCategories?
					.Select(pc => _unitOfWork.GetRepository<Category>().Entities
						.Where(c => c.Id == pc.CategoryId && !c.Style)
						.Select(c => new
						{
							c.Id,
							c.Name
						})
						.FirstOrDefault())
					.Where(c => c != null)
					.ToList(),
				Reviews = product.ProductReviews?
					.Select(pr => new
					{
						pr.Comment,
						pr.Star,
						Customer = new { pr.Customer?.Id, pr.Customer?.Name },
						Date = pr.Time.ToString("dd/MM/yyyy")
					})
					.OrderByDescending(pr => pr.Date)
					.ToList()
			};


			return responseItem;
		}

		private async Task<object> GetDesign(Product product)
		{
			await _unitOfWork.SaveAsync();

			var responseItem = new
			{
				product.Id,
				product.Name,
				product.Description,
				product.Rating,
				product.Price,
				product.Active,
				Designer = new { product.Designer?.Id, product.Designer?.Name },
				Furnitures = product.Furnitures?
					.Select(f => new
					{
						f.Id,
						f.Name,
						PrimaryImage = new { f.Images?.FirstOrDefault(img => img.IsPrimary)?.ImageSource }
					})
					.ToList(),
				PrimaryImage = new { product.Images?.FirstOrDefault(img => img.IsPrimary)?.ImageSource },
				Images = product.Images?
					.Where(img => !img.IsPrimary)
					.Select(img => new { img.ImageSource })
					.ToList(),
				Style = product.ProductCategories?
					.Select(pc => _unitOfWork.GetRepository<Category>().Entities
						.Where(c => c.Id == pc.CategoryId)
						.Select(c => new { c.Id, c.Name, c.Style })
						.FirstOrDefault())
					.Where(c => c != null)
					.OrderByDescending(c => c?.Style)
					.Select(c => new { c?.Id, c?.Name })
					.FirstOrDefault(),
				Categories = product.ProductCategories?
					.Select(pc => _unitOfWork.GetRepository<Category>().Entities
						.Where(c => c.Id == pc.CategoryId && !c.Style)
						.Select(c => new
						{
							c.Id,
							c.Name
						})
						.FirstOrDefault())
					.Where(c => c != null)
					.ToList(),
				Reviews = product.ProductReviews?
					.Select(pr => new
					{
						pr.Comment,
						pr.Star,
						Customer = new { pr.Customer?.Id, pr.Customer?.Name },
						Date = pr.Time.ToString("dd/MM/yyyy")
					})
					.OrderByDescending(pr => pr.Date)
					.ToList()
			};


			return responseItem;
		}

	}
}