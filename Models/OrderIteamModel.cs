using System;
namespace Hotel.Models
{
	public class OrderIteamModel
	{
        public Guid UserId { get; set; }

        public Guid RoomId { get; set; }

        public Guid OrderIteamId { get; set; }

        public int Count { get; set; }

        public int Price { get; set; }
        
    }
}

