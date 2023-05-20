using System;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Hotel.DataBase;
using Hotel.Models;
using Hotel.Context;

namespace Hotel.Controllers
{
	[ApiController]
	[Route("OrderAction")]
	public class OrderAction : ControllerBase
	{
		public ShopContext _context;

		private Guid UserId => Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

		public int _promocode;

		public OrderAction(ShopContext context)
		{
			_context = context;

		}
		[HttpPost("CreatenewOrder")]
		public IActionResult CreateNewOrder(NewOrderModel newOrder)
		{
			Guid guid = Guid.NewGuid();

			int TotalPrice = 0;

			var user = _context.users
				.Where(x => x.UserId == UserId)
				.Include(y => y.CartIteams)
				.ThenInclude(c => c.Room)
				.FirstOrDefault();

			var promocode = _context.promocodes.FirstOrDefault(x => x.PromocodeName == newOrder.Promocode);

			int Promocode = 0;

			string PromocodeName = "";

			if (promocode != null)
			{
				Promocode = promocode.Sum;

				PromocodeName = promocode.PromocodeName;
			}
			if (user != null)
			{

				foreach (var cartIteam in user.CartIteams)
				{
					_context.orderIteams.Add(new OrderIteam
					{
						Count = cartIteam.Count,
						OrderId = guid,
						OrderIteamId = Guid.NewGuid(),
						Price = cartIteam.Room.Price,
						RoomId = cartIteam.RoomId,
						UserId = UserId
					});

					//_context.SaveChanges();

					TotalPrice += cartIteam.Room.Price * cartIteam.Count;

					cartIteam.Room.Count -= cartIteam.Count;

				}
				if (TotalPrice > Promocode)
				{
					_context.orders.Add(new Order
					{
						OrderId = guid,
						OrderTime = DateTime.Now.ToString(),
						TotalPrice = TotalPrice - Promocode,
						UserId = UserId,
						Promocode = PromocodeName
					});
				}
				else if(TotalPrice <= Promocode)
				{
                    _context.orders.Add(new Order
                    {
                        OrderId = guid,
                        OrderTime = DateTime.Now.ToString(),
                        TotalPrice = 0,
                        UserId = UserId,
                        Promocode = PromocodeName
                    });
                }

				var deleteuser = _context.cartIteams.Where(x => x.UserId == UserId);

				_context.cartIteams.RemoveRange(deleteuser);

				_context.SaveChanges();

				return Ok("Successful");
			}
			else
			{
				return BadRequest("Op's");
			}
		}

		[HttpGet("ShowUserOrderIteam")]
        public IActionResult ShowUserOrderIteam(Guid UserId)
        {
            var user = _context.orderIteams.Where(x => x.UserId == UserId);

            if (user != null)
            {
				return Ok(user);
            }
            else
            {
                return BadRequest("You don't have any orderiteam!");
            }
        }

		[HttpGet("ShowUserOrder")]
        public IActionResult ShowUserOrder()
        {
            var user = _context.orders.Where(x => x.UserId == UserId);

            if (user != null)
            {
				return Ok(user);
            }
            else
            {
                return BadRequest("You don't have any order!");
            }
        }

		[HttpGet("ShowAllOrder")]
        public IActionResult ShowOrder()
		{
			return Ok(_context.orders);
		}

		[HttpGet("ShowAllCartIteam")]
        public IActionResult ShowOrderIteam()
        {
			return Ok(_context.cartIteams);
        }
    }
}

