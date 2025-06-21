using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SnapRoom.Common.Enum;
using SnapRoom.Contract.Repositories.Entities;
using SnapRoom.Contract.Repositories.IUOW;
using SnapRoom.Contract.Services;
using System.ComponentModel.DataAnnotations;
using static SnapRoom.Common.Base.BaseException;

namespace SnapRoom.Services
{
	public class ConversationService : IConversationService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IAuthService _authService;

		public ConversationService(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_authService = authService;
		}

		public async Task<object> GetMessages(string id)
		{
			var messages = await _unitOfWork.GetRepository<Message>().Entities
				.Where(m => m.ConversationId == id)
				.OrderBy(m => m.CreatedTime)
				.Select(m => new
				{
					m.SenderId,
					m.Content,
					m.ConversationId,
					CreatedTime = m.CreatedTime.ToString("o") // Send as string for JSON compatibility
				})
				.ToListAsync();

			return messages;
		}

		public async Task<object> GetConversations()
		{
			string userId = _authService.GetCurrentAccountId();

			Account? user = await _unitOfWork.GetRepository<Account>().Entities
				.Where(a => a.Id == userId).FirstOrDefaultAsync();

			if (user == null)
			{
				throw new ErrorException(404, "", "Tài khoản không tồn tại");
			}

			var conversations = await _unitOfWork.GetRepository<Conversation>().Entities
				.Where(c => c.DesignerId == userId || c.CustomerId == userId)
				.Select(c => new
				{
					c.Id,
					LastMessage = c.Messages!.OrderByDescending(m => m.CreatedTime).FirstOrDefault()!.Content,
					LastMessageTime = c.Messages!.OrderByDescending(m => m.CreatedTime).FirstOrDefault()!.CreatedTime.ToString("o"),
					Sender = new
					{
						Name = c.DesignerId == userId ? c.Customer!.Name : c.Designer!.Name,
						Avatar = c.DesignerId == userId ? c.Customer!.AvatarSource : c.Designer!.AvatarSource
					}

				})
				.ToListAsync();
			return conversations;
		}
	}
}
