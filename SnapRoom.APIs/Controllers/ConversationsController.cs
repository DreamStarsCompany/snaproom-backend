using Microsoft.AspNetCore.Mvc;
using SnapRoom.Contract.Services;

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
	}
}
