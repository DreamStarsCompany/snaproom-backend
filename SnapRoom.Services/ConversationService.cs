using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SnapRoom.Contract.Repositories.Entities;
using SnapRoom.Contract.Repositories.IUOW;
using SnapRoom.Contract.Services;

namespace SnapRoom.Services
{
	public class ConversationService : IConversationService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IAuthService _authenticationService;

		public ConversationService(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authenticationService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_authenticationService = authenticationService;
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
	}
}
