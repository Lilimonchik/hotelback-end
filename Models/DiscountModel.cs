using System;
namespace Hotel.Models
{
	public class DiscountModel
	{
		public Guid UserId { get; set; }

		public Guid RoomId { get; set; }

		public int SumDiscount { get; set; }

		public string TypeDiscount { get; set; }

	}
}

