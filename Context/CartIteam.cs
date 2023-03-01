using System;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Context
{
    
    public class CartIteam 
	{
		public Guid UserId { get; set; }

		public Guid RoomId { get; set; }

		public int Count { get; set; }

		public User User { get; set; }
	}
}

