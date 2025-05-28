using Microsoft.AspNetCore.Mvc;
using SnapRoom.Contract.Services;

namespace SnapRoom.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PlansController : ControllerBase
	{
		private readonly IPlanService _planService;

		public PlansController(IPlanService planService)
		{
			_planService = planService;
		}
	}
}
