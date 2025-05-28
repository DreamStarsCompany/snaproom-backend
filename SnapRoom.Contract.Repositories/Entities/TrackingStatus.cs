using SnapRoom.Common.Utils;

namespace SnapRoom.Contract.Repositories.Entities
{
	public class TrackingStatus
	{
		public DateTimeOffset Time { get; set; } = CoreHelper.SystemTimeNow;

		public string? StatusId { get; set; }
		public virtual Status? Status { get; set; }
		public string? OrderId { get; set; }
		public virtual Order? Order { get; set; }
	}
}
