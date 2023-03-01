using System;
namespace Hotel.Models
{
	public class CartIteamModel
	{
        public Guid UserId { get; set; }

        public Guid RoomId { get; set; }

        public int Count { get; set; }
    }
}

