using System;
using Hotel.Context;
namespace Hotel.Context
{
	public class RoomDTO
	{
		public string CategoryName { get; set; }

        public int Price { get; set; }

        public int Discount { get; set; }
    }
}

