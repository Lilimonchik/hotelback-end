using System;
using Hotel.Context;
using Hotel.DataBase;
using Hotel.Interfaces;
using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
namespace Hotel.Controllers
{
    [ApiController]
    [Route("RegistrationAction")]
	public class RegistrationAction : ControllerBase
	{
		public MainInterface _context;

		public RegistrationAction(MainInterface context)
		{
			_context = context;
		}

        [HttpPost("RegistrationNewUser")]
        public IActionResult RegistrationNewUser([FromBody]RegistrationModel args)
        {
            var user = _context.users.FirstOrDefault(x => x.Name == args.Name);

            if (user == null)
            {
                _context.users.Add(new User
                {
                    Name = args.Name,
                    Password = args.Password,
                    UserId = Guid.NewGuid(),
                    Role = Role.User
                });
                _context.users.SaveChange();

                return Ok("Successful!");
            }
            else
            {
                return BadRequest("Op's! This Username has already use!");
            }

        }
        [HttpGet("ShowUser")]
        public void ShowUser()
        {
            foreach (User user in _context.users)
            {
                Console.WriteLine("Name: {0}  Password: {1}   UserId: {2}  Online: {3}  Role: {4} \n\n", user.Name, user.Password, user.UserId, user.Online, user.Role);
            }
        }

    }
}

