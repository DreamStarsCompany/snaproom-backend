using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Cms;
using SnapRoom.Contract.Repositories.Entities;
using SnapRoom.Contract.Repositories.IUOW;
using SnapRoom.Contract.Services;

namespace SnapRoom.Services
{
	public class DashboardService : IDashboardService
	{
		private readonly IUnitOfWork _unitOfWork;

		public DashboardService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<object> GetRevenueByDay(int month, int year)
		{
			var currentYear = DateTime.Now.Year;

			List<Order> orders = await _unitOfWork.GetRepository<Order>().Entities
				.Where(o => !o.IsCart && o.TrackingStatuses != null && o.TrackingStatuses.Any(ts => ts.StatusId == "1" && ts.Time.Month == month && ts.Time.Year == year)).ToListAsync();

			int daysInMonth = DateTime.DaysInMonth(currentYear, month);


			var result = Enumerable.Range(1, daysInMonth)
				.Select(day => new
				{
					day = day,
					revenue = orders
						.Where(x =>
							x.TrackingStatuses.FirstOrDefault(ts => ts.Status.Name == "Processing")?.Time.Day == day)
						.Sum(x => x.OrderPrice)
				})
				.ToList();

			return result;
		}


	}
}
