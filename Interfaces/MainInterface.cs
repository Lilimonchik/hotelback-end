using System;
using Hotel.Context;
using Microsoft.AspNetCore.Mvc;
namespace Hotel.Interfaces
{
	public interface MainInterface
	{
		public List<User> users { get; set; }

		public List<Room> rooms { get; set; }

		public List<Category> categories { get; set; }

		public List<CartIteam> cartIteams { get; set; }

		public List<Promocode> promocodes { get; set; }

		public List<OrderIteam> orderIteams { get; set; }

		public List<Order> orders { get; set; }
	}
}

