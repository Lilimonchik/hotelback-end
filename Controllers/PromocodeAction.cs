using System;
using Hotel.DataBase;
using Hotel.Interfaces;
using Hotel.Context;
using Hotel.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Controllers
{
    [ApiController]
    [Route("PromocodeAction")]
	public class PromocodeAction : ControllerBase
	{
		public ShopContext _context;

		public PromocodeAction(ShopContext context)
		{
			_context = context;
		}

        [HttpPost("AddPromocode")]
        public IActionResult AddPromocode(PromocodeModel args)
        {
            var user = _context.users.FirstOrDefault(x => x.UserId == args.UserId);

            var promocode = _context.promocodes.FirstOrDefault(y => y.PromocodeName == args.NamePromocode);

            if (user != null && promocode == null)
            {
                if (user.Role == Role.Admin)
                {
                    _context.promocodes.Add(new Promocode
                    {
                        Sum = args.Sum,
                        PromocodeName = args.NamePromocode
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

        [HttpGet("ShowPromocode")]
        public IActionResult ShowPromocode()
        {
            return Ok(_context.promocodes);
        }
    }
}

