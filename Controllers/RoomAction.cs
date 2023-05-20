using System;
using System.Security.Claims;
using Hotel.Context;
using Hotel.DataBase;
using Hotel.Interfaces;
using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    [ApiController]
    [Route("RoomAction")]
	public class RoomAction : ControllerBase
	{
		public ShopContext _context;

		public RoomAction(ShopContext context)
		{
			_context = context;
		}

        private Guid UserId => Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpPost("AddRoom")]
        public IActionResult AddRoom(RoomModel argsAddRoom)
        {
            var user = _context.users
                .Where(x => x.UserId == UserId)
                .Include(c => c.CartIteams)
                .ThenInclude(y => y.Room)
                .FirstOrDefault();

            var categorymain = _context.categories
                .Where(x => x.CategoryName == argsAddRoom.Category)
                .FirstOrDefault();

            if (categorymain != null)
            {
                if (user != null)
                {
                    if (user.Role == Role.Admin)
                    {
                        _context.rooms.Add(new Room
                        {
                            Price = argsAddRoom.Price,
                            CountOfPeople = argsAddRoom.CountPeople,
                            CountOfRoom = argsAddRoom.CountRoom,
                            RoomId = Guid.NewGuid(),
                            CategoryForId = categorymain.CategoryId,
                            Count = argsAddRoom.Count,
                            Discount = 0
                        });

                        _context.SaveChanges();

                        return Ok("Successful!");
                    }
                    else
                    {
                        return BadRequest("Op's! You're not an admin or not online!");
                    }
                }
                else
                {
                    return BadRequest("Op's! Error!");
                }
            }
            else
            {
                if (user != null)
                {
                    if (user.Role == Role.Admin)
                    {
                        _context.rooms.Add(new Room
                        {
                            Price = argsAddRoom.Price,
                            CountOfPeople = argsAddRoom.CountPeople,
                            CountOfRoom = argsAddRoom.CountRoom,
                            RoomId = Guid.NewGuid(),
                            Category = new Category
                            {
                                CategoryId = Guid.NewGuid(),
                                CategoryName = argsAddRoom.Category,
                            },
                            Count = argsAddRoom.Count,
                            Discount = 0
                        });

                        _context.SaveChanges();

                        return Ok("Successful!");
                    }
                    else
                    {
                        return BadRequest("Op's! You're not an admin or not online!");
                    }
                }
                else
                {
                    return BadRequest("Op's! Error!");
                }
            }
            return Ok();
        }

        [HttpPost("AddDiscount")]
        public IActionResult AddDiscount (DiscountModel discount)
        {
            var user = _context.users.FirstOrDefault(x => x.UserId == UserId);

            var room = _context.rooms.FirstOrDefault(x => x.RoomId == discount.RoomId);

            if(user != null && room != null)
            {
                if(user.Role == Role.Admin)
                {
                    if (discount.TypeDiscount == "Interest" && ((discount.SumDiscount * room.Price) / 100) <= room.Price)
                    {
                        room.Discount = discount.SumDiscount;

                        room.Price -= ((room.Price * discount.SumDiscount) / 100);

                        _context.SaveChanges();

                        return Ok("Successful!");
                    }
                    else if (discount.TypeDiscount == "Sum" && discount.SumDiscount <= room.Price)
                    {
                        room.Discount = discount.SumDiscount;

                        room.Price -= discount.SumDiscount;

                        _context.SaveChanges();

                        return Ok("Successful!");
                    }
                    else
                    {
                        return BadRequest("Op's! Error!");
                    }
                }
            }
            return BadRequest("Op's! Incorrect data!");
        }

        [HttpGet("ShowRoom")]
        public IActionResult ShowRoom()
        {
            List<RoomDTO> room = new List<RoomDTO>();

            _context.categories.Load();

            foreach (var rooms in _context.rooms)
            {
                room.Add(new RoomDTO
                {
                    CategoryName = rooms.Category.CategoryName,
                    Discount = rooms.Discount,
                    Price = rooms.Price,
                    RoomId = rooms.RoomId.ToString()

                });
            }

            return Ok(room);
        }
	}
}