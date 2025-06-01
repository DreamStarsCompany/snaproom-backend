using Microsoft.AspNetCore.Mvc;
using SnapRoom.Common.Base;
using SnapRoom.Common.Enum;
using SnapRoom.Contract.Repositories.Dtos.OrderDtos;
using SnapRoom.Contract.Services;
using SnapRoom.Services;

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

		[HttpGet("customer")]
		public async Task<IActionResult> GetOrdersForCustomer(int pageNumber = -1, int pageSize = -1)
		{
			var orders = await _orderService.GetOrdersForCustomer(pageNumber, pageSize);

			return Ok(new BaseResponse<object>(
				statusCode: StatusCodeEnum.OK,
				message: "Lấy đơn hàng thành công",
				data: orders
			));
		}

		[HttpGet("designer")]
		public async Task<IActionResult> GetOrdersForDesigner(int pageNumber = -1, int pageSize = -1)
		{
			var orders = await _orderService.GetOrdersForDesigner(pageNumber, pageSize);

			return Ok(new BaseResponse<object>(
				statusCode: StatusCodeEnum.OK,
				message: "Lấy đơn hàng thành công",
				data: orders
			));
		}


		[HttpGet("{id}")]
		public async Task<IActionResult> GetOrderById(string id)
		{
			var order = await _orderService.GetOrderById(id);

			return Ok(new BaseResponse<object>(
				statusCode: StatusCodeEnum.OK,
				message: "Lấy đơn hàng thành công",
				data: order
			));
		}

		[HttpGet("cart")]
		public async Task<IActionResult> GetCart()
		{
			var cart = await _orderService.GetCart();

			return Ok(new BaseResponse<object>(
				statusCode: StatusCodeEnum.OK,
				message: "Lấy giỏ hàng thành công",
				data: cart
			));
		}

		[HttpPost("cart")]
		public async Task<IActionResult> AddToCart(CartItemDto dto)
		{
			await _orderService.AddToCart(dto);

			return Ok(new BaseResponse<object>(
				statusCode: StatusCodeEnum.OK,
				message: "Thêm sản phẩm thành công",
				data: null
			));
		}

		[HttpDelete("cart")]
		public async Task<IActionResult> DeleteFromCart(string productId)
		{
			await _orderService.DeleteFromCart(productId);

			return Ok(new BaseResponse<object>(
				statusCode: StatusCodeEnum.OK,
				message: "Xóa sản phẩm thành công",
				data: null
			));
		}

	}
}
