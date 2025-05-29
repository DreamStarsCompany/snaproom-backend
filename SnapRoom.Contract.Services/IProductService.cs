using SnapRoom.Common.Base;
using SnapRoom.Contract.Repositories.Entities;

namespace SnapRoom.Contract.Services
{
	public interface IProductService
	{
		Task<BasePaginatedList<object>> GetFurnitures(int pageNumber, int pageSize);
		Task<BasePaginatedList<object>> GetDesigns(int pageNumber, int pageSize);

	}
}
