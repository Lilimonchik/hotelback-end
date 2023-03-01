using System;
using Hotel.Interfaces;
using Hotel.DataBase;
using Hotel.Context;
using Hotel.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Controllers
{
    [ApiController]
    [Route("LogInAction")]
    public class LogInAction : ControllerBase
    {
        public MainInterface _context;

        public LogInAction(MainInterface context)
        {
            _context = context;
        }

        [HttpPost("LogIn")]
        public IActionResult CheckUser(LogInModel args)
        {
            var user = _context.users.FirstOrDefault(x => x.Name == args.Name);

            if (user != null &&  user.Online == false)
            {
                if (user.Name == args.Name && user.Password == args.Password)
                {
                    user.Online = true;

                    return Ok("Successful!");
                }
            }
            else
                {
                    return BadRequest("Op's! We don't have a user with this name or you have already log in!");
                }
            return Ok();
            }
        }
    }