using System;
namespace Hotel.Context
{
	public class Room
	{
		public Guid RoomId { get; set; }

		public int Price { get; set; }

		public int CountOfPeople { get; set; }

		public int CountOfRoom { get; set; }

		public int Count { get; set; }

		public int Discount { get; set; }

		public Guid CategoryForId { get; set; }

		public ICollection<CartIteam> CartIteams { get; set; }

		public Category Category { get; set; }

		public ICollection<OrderIteam> OrderIteams { get; set; }

	}
}