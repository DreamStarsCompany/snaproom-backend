using SnapRoom.Contract.Repositories.IUOW;

namespace SnapRoom.Services
{
	public class DashboardService
	{
		private readonly IUnitOfWork _unitOfWork;

		public DashboardService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
	}
}
