using SnapRoom.Common.Base;
using SnapRoom.Common.Enum;

namespace SnapRoom.Contract.Services
{
	public interface IAccountService
	{
		Task<BasePaginatedList<object>> GetAccounts(RoleEnum? role, int pageNumber, int pageSize);
	}
}
