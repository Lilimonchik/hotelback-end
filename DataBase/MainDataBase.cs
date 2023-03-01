using System;
using Hotel.Context;
using Hotel.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace Hotel.DataBase
{
	public class MainDataBase : MainInterface
	{
		public List<User> users { get; set; } = new List<User>();

		public List<Room> rooms { get; set; } = new List<Room>();

		public List<Category> categories { get; set; } = new List<Category>();

		public List<CartIteam> cartIteams { get; set; } = new List<CartIteam>();

		public List<Promocode> promocodes { get; set; } = new List<Promocode>();

		public List<OrderIteam> orderIteams { get; set; } = new List<OrderIteam>();

		public List<Order> orders { get; set; } = new List<Order>();

	}
}

