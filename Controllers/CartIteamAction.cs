using System;
using Hotel.Context;
using Hotel.DataBase;
using Hotel.Interfaces;
using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    [ApiController]
    [Route("CartIteamAction")]
	public class CartIteamAction : ControllerBase
	{
		public ShopContext _context;

		public CartIteamAction(ShopContext context)
		{
			_context = context;
		}

        private Guid UserId => Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpPost("AddCartIteam")]

        public IActionResult AddCartIteam(CartIteamModel args)
        {
            Guid RoomId = Guid.Parse(args.RoomId.ToString());

            var user = _context.users.FirstOrDefault(x => x.UserId == UserId);

            var room = _context.rooms.FirstOrDefault(x => x.RoomId == RoomId);

            if (user != null && room != null)
            {
                if (room.Count >= args.Count)
                {
                    _context.cartIteams.Add(new CartIteam
                    {
                        Count = args.Count,
                        CartIteamId = Guid.NewGuid(),
                        RoomId = RoomId,
                        UserId = UserId,
                    });
                    _context.SaveChanges();

                    return Ok("Successful!");
                }
                else
                {
                    return BadRequest("Op's! You're not online or not enough rooms!");
                }
            }
            else
            {
                return BadRequest("Op's! You're not online or not enough rooms!");
            }
        }

        [HttpGet("ShowUserCartIteam")]

        public IActionResult ShowUserCartIteam()
        {
            List<CartIteamDTO> cartIteams = new List<CartIteamDTO>();

            _context.rooms.Load();

            foreach (var iteam in _context.cartIteams)
            {
                if (iteam.UserId == UserId)
                {
                    var price = iteam.Room.Price;
                    cartIteams.Add(new CartIteamDTO
                    {
                        CartIteamId = iteam.CartIteamId.ToString(),
                        Count = iteam.Count,
                        RoomId = iteam.RoomId.ToString(),
                        UserId = iteam.UserId.ToString(),
                        Price = iteam.Room.Price
                    });
                }
            }
            return Ok(cartIteams);
        }
        [HttpGet("ShowCartIteam")]
        public IActionResult ShowCartIteam()
        {
            return Ok(_context.cartIteams);
        }
    }
}

