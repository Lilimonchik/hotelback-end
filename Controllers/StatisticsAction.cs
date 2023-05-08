using System;
using System.Collections.Generic;
using Hotel.DataBase;
using Hotel.Models;
using Hotel.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
	[ApiController]
	[Route("Statistics")]
	public class StatisticsAction :ControllerBase
	{
		public ShopContext _context;

		public StatisticsAction(ShopContext context)
		{
			_context = context;
		}
		[HttpGet("UserThatSpendTheMost")]
		public IActionResult UserThatSpendTheMost()
		{
			var price = _context.orders.Max(x => x.TotalPrice);

			var user = _context.orders.FirstOrDefault(y => y.TotalPrice == price);

			return Ok(user.UserId);
		}

		[HttpGet("UserThatOrderedTheMost")]
		public IActionResult UserThatOrderedTheMost()
		{
            var most = _context.orders.GroupBy(i => i.UserId).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();

			return Ok(most);
        }

		[HttpGet("TheMostPopularRoom")]
		public IActionResult TheMostPopularRoom()
		{
			var room = _context.orderIteams.GroupBy(i => i.RoomId).OrderByDescending(u => u.Count()).Select(k => k.Key).First();

			return Ok(room);
		}

		[HttpGet("TheMostPopularPromocode")]
		public IActionResult TheMostPopularPromocode()
		{
			var promocode = _context.orders.GroupBy(i => i.Promocode).OrderByDescending(u => u.Count()).Select(k => k.Key).First();

			return Ok(promocode);
		}

		[HttpGet("TheMostPopularInAnyCategory")]
		public IActionResult TheMostPopularInStandart()
		{
			List<CartIteam> cartIteams = new List<CartIteam>();
			var standartid = _context.categories.FirstOrDefault(x => x.CategoryName == "Standart");

			var roomid = _context.rooms
				.Where(x => x.Category.CategoryId == standartid.CategoryId)
				.Include(c => c.CartIteams)
				.ThenInclude(v => v.User);

			foreach(var cart in roomid)
			{
				
			}

			return Ok();
		}
		[HttpGet("AllBuys")]
		public IActionResult AllBuys()
		{
			var buys = _context.orders.Count();

			return Ok(buys);
		}

		[HttpGet("CountOfUser")]
		public IActionResult CountOfUser()
		{
			var user = _context.users.Count();

			return Ok(user);
		}

		[HttpGet("HowManyRoomWasBooked")]
		public IActionResult HowManyRoomWasBooked()
		{
			var room = _context.orderIteams.Sum(x => x.Count);

			return Ok(room);
		}
    }
}

