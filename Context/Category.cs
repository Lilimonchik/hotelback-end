using System;
namespace Hotel.Context
{
	public class Category
	{
		public Guid CategoryId { get; set; }

		public string CategoryName { get; set; }

		public ICollection<Room> Rooms { get; set; }
    }
}

