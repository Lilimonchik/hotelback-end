using System;
namespace Hotel.Context
{
	public class Room
	{
		public Guid IdRoom { get; set; }

		public Category Category { get; set; }

		public int Price { get; set; }

		public int CountOfPeople { get; set;}

		public int CountOfRoom { get; set; }

		public int Count { get; set; }

		public int Discount { get; set; }

		public string TypeDiscount { get; set; }
	}
}