namespace SnapRoom.Contract.Services
{
	public interface IDashboardService
	{
		Task<object> GetRevenueByDay(int month, int year);
	}
}
