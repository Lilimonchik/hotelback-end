using System;
using Hotel.DataBase;
using Hotel.Interfaces;
using Hotel.Context;
using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Hotel.Controllers
{
	[ApiController]
	[Route("AdminAction")]
    public class AdminAction : ControllerBase
	{
		public MainInterface _context;

		public AdminAction(MainInterface context)
		{
			_context = context;
		}

		[HttpPost("AddAdmin")]
		public IActionResult AddNewAdmin(AdminModel args)
		{
			var user = _context.users.FirstOrDefault(x => x.UserId == args.UserId);

			if (args.AdminPassword == "4590" && user.Online == true && user.Role != Role.Admin)
			{
				user.Role = Role.Admin;

				return Ok("Successful!");
			}
			else
			{
				return BadRequest("Op's! Incorrect password or you are not online or an admin!");
			}
		}

        [HttpGet("ShowAdmin")]

        public IActionResult ShowAdmin(AdminModel args)
		{
			var userAdmin = _context.users.FirstOrDefault(x => x.UserId == args.UserId);

			if (userAdmin.Role == Role.Admin)
			{
				foreach (User user in _context.users)
				{
					if (user.Role == Role.Admin)
					{
						Console.WriteLine("{0} is Admin\n", user.Name);
					}
				}
				return Ok("Successful!");
			}
			return Ok();
		}
    }
}