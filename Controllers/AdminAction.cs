using System;
using Hotel.DataBase;
using Hotel.Context;
using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Cors;

namespace Hotel.Controllers
{
	[EnableCors("MyPolicy")]
	[ApiController]
	[Route("AdminAction")]
    public class AdminAction : ControllerBase
	{
		public ShopContext _context;

		public AdminAction(ShopContext context)
		{
			_context = context;
		}

		[HttpPost("AddAdmin")]
		public IActionResult AddNewAdmin(AdminModel args)
		{
			var user = _context.users.FirstOrDefault(x => x.UserId == args.UserId);

			var adminuser = _context.users.FirstOrDefault(t => t.UserId == args.AdminId);

			if (user != null && adminuser != null)
			{

				if (adminuser.Role == Role.Admin && user.Role != Role.Admin)
				{
					user.Role = Role.Admin;

					_context.SaveChanges();

					return Ok("Successful!");
				}
				else
				{
					return BadRequest("Op's! Incorrect password or you are not online or an admin!");
				}
			}
			else
			{
				return BadRequest("Op's! Error");
			}
		}
        [HttpGet("ShowAdmin")]

        public IActionResult ShowAdmin(AdminModel args)
		{
			var userAdmin = _context.users.FirstOrDefault(x => x.UserId == args.UserId);

			if(userAdmin.Role == Role.Admin)
			{
				return Ok(userAdmin);
			}
			else
			{
				return BadRequest("Op's! You are not an andmin or you are not online!");
			}
		}
    }
}