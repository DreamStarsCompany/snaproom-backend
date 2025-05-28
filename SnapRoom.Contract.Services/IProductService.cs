using SnapRoom.Common.Base;
using SnapRoom.Contract.Repositories.Entities;

namespace SnapRoom.Contract.Services
{
	public interface IProductService
	{
		Task<BasePaginatedList<object>> GetProducts(int pageNumber, int pageSize);
	}
}
