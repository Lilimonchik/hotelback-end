using System;
namespace Hotel.Context
{
	public class OrderIteam
	{
        public Guid UserId { get; set; }

        public Guid RoomId { get; set; }

        public Guid OrderIteamId { get; set; }

        public Guid OrderId { get; set; }

        public int Count { get; set; }

        public int Price { get; set; }

        public Order Order { get; set; }
    }
}

