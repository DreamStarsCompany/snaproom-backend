namespace SnapRoom.Contract.Services
{
	public interface IConversationService
	{
		Task<object> GetMessages(string id);
		Task<object?> GetConversation(string otherUserId);
		Task<object> GetConversations();
	}
}
