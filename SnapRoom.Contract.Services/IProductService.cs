using SnapRoom.Common.Base;
using SnapRoom.Contract.Repositories.Dtos.ProductDtos;
using SnapRoom.Contract.Repositories.Entities;

namespace SnapRoom.Contract.Services
{
	public interface IProductService
	{
		Task<BasePaginatedList<object>> GetFurnitures(int pageNumber, int pageSize);
		Task<BasePaginatedList<object>> GetDesigns(int pageNumber, int pageSize);
		Task<BasePaginatedList<object>> GetFurnituresForDesigner(int pageNumber, int pageSize);
		Task<BasePaginatedList<object>> GetDesignsForDesigner(int pageNumber, int pageSize);
		Task<object> GetProductById(string id);
		Task CreateDesign(DesignCreateDto dto);
	}
}
