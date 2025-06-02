using Microsoft.AspNetCore.Mvc;
using SnapRoom.Common.Base;
using SnapRoom.Common.Enum;
using SnapRoom.Contract.Repositories.Dtos.ProductDtos;
using SnapRoom.Contract.Services;

namespace SnapRoom.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly IProductService _productService;

		public ProductsController(IProductService productService)
		{
			_productService = productService;
		}

		[HttpGet("designs")]
		public async Task<IActionResult> GetDesigns(int pageNumber = -1, int pageSize = -1)
		{
			var products = await _productService.GetDesigns(pageNumber, pageSize);

			return Ok(new BaseResponse<object>(
				statusCode: StatusCodeEnum.OK,
				message: "Lấy sản phẩm thành công",
				data: products
			));
		}

		[HttpGet("furnitures")]
		public async Task<IActionResult> GetFurnitures(int pageNumber = -1, int pageSize = -1)
		{
			var products = await _productService.GetFurnitures(pageNumber, pageSize);

			return Ok(new BaseResponse<object>(
				statusCode: StatusCodeEnum.OK,
				message: "Lấy sản phẩm thành công",
				data: products
			));
		}

		[HttpPost("designs")]
		public async Task<IActionResult> CreateDesign(DesignCreateDto dto)
		{
			await _productService.CreateDesign(dto);

			return Ok(new BaseResponse<object>(
				statusCode: StatusCodeEnum.OK,
				message: "Tạo bản thiết kế thành công",
				data: null
			));
		}


	}
}
