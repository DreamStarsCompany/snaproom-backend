using SnapRoom.Common.Base;

namespace SnapRoom.Contract.Repositories.Entities
{
	public class Category : BaseEntity
	{
		public string Name { get; set; } = default!;
	}
}
