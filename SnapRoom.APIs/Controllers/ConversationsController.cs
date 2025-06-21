using Microsoft.AspNetCore.Mvc;
using SnapRoom.Common.Base;
using SnapRoom.Common.Enum;
using SnapRoom.Contract.Services;
using SnapRoom.Services;

namespace SnapRoom.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ConversationsController : ControllerBase
	{
		private readonly IConversationService _conversationService;

		public ConversationsController(IConversationService conversationService)
		{
			_conversationService = conversationService;
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetMessages(string id)
		{
			var result = await _conversationService.GetMessages(id);
			return Ok(new BaseResponse<object>(
				statusCode: StatusCodeEnum.OK,
				message: "Lấy đoạn chat thành công",
				data: result
			));
		}

	}
}
