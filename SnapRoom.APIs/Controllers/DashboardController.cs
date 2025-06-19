using Microsoft.AspNetCore.Mvc;
using SnapRoom.Common.Base;
using SnapRoom.Common.Enum;
using SnapRoom.Contract.Services;

namespace SnapRoom.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DashboardController : ControllerBase
	{
		private readonly IDashboardService _dashboardService;

		public DashboardController(IDashboardService dashboardService)
		{
			_dashboardService = dashboardService;
		}

		[HttpGet("revenue-by-day")]
		public async Task<IActionResult> GetRevenueByDay(int month, int year)
		{
			var result = await _dashboardService.GetRevenueByDay(month, year);
			return Ok(new BaseResponse<object>(
				statusCode: StatusCodeEnum.OK,
				message: "Lấy dữ liệu thành công",
				data: result
			));
		}

	}
}
