using System;
using Hotel.Context;
using Hotel.DataBase;
using Hotel.Interfaces;
using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

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
            var user = _context.users.FirstOrDefault(x => x.UserId == args.UserId);

            var room = _context.rooms.FirstOrDefault(x => x.RoomId == args.RoomId);

            if (user != null && room != null)
            {
                if (room.Count >= args.Count)
                {
                    _context.cartIteams.Add(new CartIteam
                    {
                        Count = args.Count,
                        RoomId = args.RoomId,
                        UserId = args.UserId
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
            var user = _context.cartIteams.Where(x => x.UserId == UserId);

            if (user != null)
            {

                return Ok(user);

            }
            else
            {
                return BadRequest("You don't have any cartiteam!");
            }
        }
        [HttpGet("ShowCartIteam")]
        public IActionResult ShowCartIteam()
        {
            return Ok(_context.cartIteams);
        }
    }
}

