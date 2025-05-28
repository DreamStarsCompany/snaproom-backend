namespace SnapRoom.Contract.Repositories.Entities
{
	public class OrderDetail
	{
		public int Quantity { get; set; }
		public decimal DetailPrice { get; set; }

		public string? OrderId { get; set; }
		public virtual Order? Order { get; set; }
		public string? ProductId { get; set; }
		public virtual Product? Product { get; set; }
	}
}
