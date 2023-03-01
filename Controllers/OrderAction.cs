using System;
using Hotel.DataBase;
using Hotel.Interfaces;
using Hotel.Context;
using Hotel.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
namespace Hotel.Controllers
{
	[ApiController]
    [Route("OrderAction")]
    public class OrderAction : ControllerBase
	{
		public MainInterface _context;

		public int _promocode;

		public OrderAction(MainInterface context,int promocode)
		{
			_context = context;

			_promocode = promocode;
		}
		[HttpPost("CreatenewOrder")]
		public IActionResult CreateNewOrder(NewOrderModel newOrder)
		{
			Guid guid = Guid.NewGuid();

			int TotalPrice = 0;

			var user = _context.users.FirstOrDefault(x => x.UserId == newOrder.UserId);

			var cartIteams = user.CartIteams;

			foreach (var rooms in cartIteams)
			{
				
				var room = _context.rooms.FirstOrDefault(x => x.IdRoom == rooms.RoomId);

				if (room != null) {

					_context.orderIteams.Add(new OrderIteam
					{
						Count = rooms.Count,
						OrderId = guid,
						OrderIteamId = Guid.NewGuid(),
						Price = room.Price,
						RoomId = rooms.RoomId,
						UserId = rooms.UserId

					});
					TotalPrice += room.Price * rooms.Count;
				}
				room.Count -= rooms.Count;
			}
			_context.orders.Add(new Order
			{
				OrderId = guid,
				OrderTime = DateTime.Now.ToShortDateString(),
				TotalPrice = TotalPrice-_promocode,
				UserId = newOrder.UserId

			});
				return Ok("Successful");
			}
		[HttpGet("ShowUserOrderIteam")]
        public void ShowUserOrderIteam(Guid UserId)
        {
            var user = _context.orderIteams.Where(x => x.UserId == UserId);

            if (user != null)
            {
                Console.WriteLine("User {0} orderiteam ", UserId);

                foreach (var users in user)
                {
					
                    Console.WriteLine("RoomId: {0} Count: {1} Price: {2} OrderId: {3} OrderIteamId: {4}  ", users.RoomId, users.Count,users.Price,users.OrderId,users.OrderIteamId);
                }
            }
            else
            {
                Console.WriteLine("You don't have any orderiteam!");
            }
        }
		[HttpGet("ShowUserOrder")]
        public void ShowUserOrder(Guid UserId)
        {
            var user = _context.orders.Where(x => x.UserId == UserId);

            if (user != null)
            {
                Console.WriteLine("User {0} order: ", UserId);

				foreach (var users in user)
				{ 
					Console.WriteLine("OrderId: {0} OrderTime: {1} Promocode: {2} TotalPrice: {3} ", users.OrderId, users.OrderTime, users.Promocode, users.TotalPrice);
				}
            }
            else
            {
                Console.WriteLine("You don't have any order!");
            }
        }
		[HttpGet("ShowAllOrder")]
        public void ShowOrder()
		{
			foreach(Order order in _context.orders)
			{
				Console.WriteLine("UserId: {0} TotalPrice: {1} OrderTime: {2} OrderId: {3}",order.UserId,order.TotalPrice,order.OrderTime,order.OrderId);
			}
		}
		[HttpGet("ShowAllCartIteam")]
        public void ShowOrderIteam()
        {
            foreach (OrderIteam orderIteam in _context.orderIteams)
            {
                Console.WriteLine("UserId: {0} Price: {1} OrderIteamId: {2} Count: {3} RoomId: {4}", orderIteam.UserId, orderIteam.Price,orderIteam.OrderIteamId,orderIteam.Count,orderIteam.RoomId);
            }
        }
    }
}

