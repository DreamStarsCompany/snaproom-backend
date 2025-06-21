using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SnapRoom.Contract.Repositories.Entities;
using SnapRoom.Contract.Repositories.IUOW;

namespace SnapRoom.APIs.Hubs
{
	public class ChatHub : Hub
	{
		private readonly IUnitOfWork _unitOfWork;

		public ChatHub(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task SendMessage(string conversationId, string senderId, string content)
		{
			Message message = new Message
			{
				ConversationId = conversationId,
				SenderId = senderId,
				Content = content,
			};

			await _unitOfWork.GetRepository<Message>().InsertAsync(message);

			await _unitOfWork.SaveAsync();

			await Clients.Group(conversationId).SendAsync("ReceiveMessage", new
			{
				senderId,
				content,
				conversationId,
				createdTime = message.CreatedTime.ToString("o") // ISO 8601 format
			});
		}
	}
}
