using System;
using Hotel.Context;
using Hotel.DataBase;
using Hotel.Interfaces;
using Hotel.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
namespace Hotel.Controllers
{
    [ApiController]
    [Route("RegistrationAction")]
	public class RegistrationAction : ControllerBase
	{
		public ShopContext _context;

		public RegistrationAction(ShopContext context)
		{
			_context = context;
		}

        [HttpPost("RegistrationNewUser")]
        public IActionResult RegistrationNewUser([FromBody]RegistrationModel args)
        {
            var user = _context.users.FirstOrDefault(x => x.UserName == args.UserName);

            if (user == null)
            {
                var sha = SHA256.Create();

                var asByteArray = Encoding.Default.GetBytes(args.Password);

                var hashedPassword = Convert.ToBase64String(sha.ComputeHash(asByteArray));

                _context.users.Add(new User
                {
                    FirstName = args.FirstName,
                    Name = args.SecondName,
                    Email = args.Email,
                    Birthday = args.Birthday,
                    UserName = args.UserName,
                    Password = hashedPassword
                }) ;

                _context.SaveChanges();

                return Ok("Successful!");
            }
            else
            {
                return BadRequest("Op's! This Username has already use!");
            }

        }
        [HttpGet("ShowUser")]
        public async Task<IActionResult> ShowUser()
        {
            return Ok(_context.users);
        }
    }
}

