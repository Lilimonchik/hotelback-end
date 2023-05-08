using System;
namespace Hotel.Context
{
	public class Order
	{
		public Guid OrderId { get; set; }

		public Guid UserId { get; set; }

		public int TotalPrice { get; set; }

		public string OrderTime { get; set; }

		public string? Promocode { get; set; }

		public User User { get; set; }

		public ICollection<OrderIteam> OrderIteams { get; set; }

	}
}

