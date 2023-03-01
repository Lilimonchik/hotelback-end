using System;
using Hotel.Context;
using Hotel.DataBase;
using Hotel.Interfaces;
using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Hotel.Controllers
{
    [ApiController]
    [Route("CartIteamAction")]
	public class CartIteamAction : ControllerBase
	{
		public MainInterface _context;

		public CartIteamAction(MainInterface context)
		{
			_context = context;
		}
        [HttpPost("AddCartIteam")]

        public IActionResult AddCartIteam(CartIteamModel args)
        {
            var user = _context.users.FirstOrDefault(x => x.UserId == args.UserId);

            var room = _context.rooms.FirstOrDefault(x => x.IdRoom == args.RoomId);

            if (user != null && room != null)
            {
                if (user.Online && room.Count >= args.Count)
                {
                    _context.cartIteams.Add(new CartIteam
                    {
                        Count = args.Count,
                        RoomId = args.RoomId,
                        UserId = args.UserId
                    });
                    return Ok("Successful!");
                }
                else
                {
                    return BadRequest("Op's! You're not online or not enough rooms!");
                }
            }
            else
            {
                return Ok("Op's! You're not online or not enough rooms!");
            }
        }

        [HttpGet("ShowUserCartIteam")]

        public IActionResult ShowUserCartIteam(Guid UserId)
        {
            var user = _context.cartIteams.Where(x => x.UserId == UserId);

            if (user != null)
            {
                Console.WriteLine("User {0} cartiteam: ",UserId);

                foreach (var users in user)
                {
                    Console.WriteLine("RoomId: {0} Count: {1} ",users.RoomId, users.Count);
                }

                return Ok("Successful!");
            }
            else
            {
                return BadRequest("You don't have any cartiteam!");
            }
        }
        [HttpGet("ShowCartIteam")]
        public void ShowCartIteam()
        {
            foreach (CartIteam cartIteam in _context.cartIteams)
            {
                Console.WriteLine("RoomId: {0}  UserId: {1} Count: {2} ", cartIteam.RoomId, cartIteam.UserId, cartIteam.Count);
            }
        }
    }
}

