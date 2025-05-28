using Microsoft.AspNetCore.Mvc;
using SnapRoom.Contract.Services;

namespace SnapRoom.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly ICategoryService _categoryService;

		public CategoriesController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}
	}
}
