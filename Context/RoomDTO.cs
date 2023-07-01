using System;
using Hotel.Context;
namespace Hotel.Context
{
	public class RoomDTO
	{
		public string CategoryName { get; set; }

        public int Price { get; set; }

        public int Discount { get; set; }

        public string RoomId { get; set; }

        public string About { get; set; }

        public int CountOfPeople { get; set; }

        public string UrlPhoto { get; set; }
    }
}

