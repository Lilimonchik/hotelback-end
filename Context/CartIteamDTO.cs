using System;
namespace Hotel.Context
{
	public class CartIteamDTO
	{
        public string CartIteamId { get; set; }

        public string UserId { get; set; }

        public string RoomId { get; set; }

        public int Count { get; set; }

        public int Price { get; set; }
    }
}

