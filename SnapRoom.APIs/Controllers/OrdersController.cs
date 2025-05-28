using Microsoft.AspNetCore.Mvc;
using SnapRoom.Contract.Services;

namespace SnapRoom.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrdersController : ControllerBase
	{
		private readonly IOrderService _orderService;
		public OrdersController(IOrderService orderService)
		{
			_orderService = orderService;
		}
	}
}
