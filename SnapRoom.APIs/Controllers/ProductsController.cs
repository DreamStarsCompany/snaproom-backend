using Microsoft.AspNetCore.Mvc;
using SnapRoom.Common.Base;
using SnapRoom.Common.Enum;
using SnapRoom.Contract.Repositories.Dtos.ProductDtos;
using SnapRoom.Contract.Services;

namespace SnapRoom.APIs.Controllers
{
	[Route("api")]
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

		[HttpGet("designer/designs")]
		public async Task<IActionResult> GetDesignsForDesigner(int pageNumber = -1, int pageSize = -1)
		{
			var products = await _productService.GetDesignsForDesigner(pageNumber, pageSize);

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

		[HttpGet("designer/furnitures")]
		public async Task<IActionResult> GetFurnituresForDesigner(int pageNumber = -1, int pageSize = -1)
		{
			var products = await _productService.GetFurnituresForDesigner(pageNumber, pageSize);

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

		[HttpPost("furnitures")]
		public async Task<IActionResult> CreateFurniture(FurnitureCreateDto dto)
		{
			await _productService.CreateFurniture(dto);

			return Ok(new BaseResponse<object>(
				statusCode: StatusCodeEnum.OK,
				message: "Tạo sản phẩm thành công",
				data: null
			));
		}


		[HttpGet("products/{id}")]
		public async Task<IActionResult> GetProductById(string id)
		{
			var product = await _productService.GetProductById(id);

			return Ok(new BaseResponse<object>(
				statusCode: StatusCodeEnum.OK,
				message: "Lấy sản phẩm thành công",
				data: product
			));
		}
	}
}
