using System;
using Hotel.Interfaces;
using Hotel.DataBase;
using Hotel.Context;
using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;

namespace Hotel.Controllers
{
    [ApiController]
    [Route("LogInAction")]
    [EnableCors("AllowOrigin")]
    public class LogInAction : ControllerBase
    {
        public ShopContext _context;

        private readonly IOptions<AuthOptions> authOptions;
        public LogInAction(ShopContext context, IOptions<AuthOptions> authOptions)
        {
            _context = context;
            this.authOptions = authOptions;
        }
        private Guid UserId => Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpPost("LogIn")]
        public IActionResult CheckUser(LogInModel args)
        {
            var user = _context.users.FirstOrDefault(x => x.UserName == args.UserName);

            if (user != null)
            {
                var sha = SHA256.Create();

                var asByteArray = Encoding.Default.GetBytes(args.Password);

                var hashedPassword = Convert.ToBase64String(sha.ComputeHash(asByteArray));

                if (user.UserName == args.UserName && user.Password == hashedPassword)
                {

                    _context.SaveChanges();

                    var token = GenerateToken(user);



                    return Ok(new { access_token = token });
                }
            }
            else
            {
                return BadRequest("Op's! We don't have a user with this name or you have already log in!");
            }
            return Ok();
        }
        private string GenerateToken(User user)
        {

            var authParams = authOptions.Value;

            var securityKey = authParams.GetSymmetricSecutityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim (JwtRegisteredClaimNames.Name,user.UserName),
                new Claim (JwtRegisteredClaimNames.Sub,user.UserId.ToString())
            };

            var token = new JwtSecurityToken(authParams.Issuer,
                authParams.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(authParams.TokenLifeTime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        [HttpGet("GetInfoAboutUser")]
        public IActionResult GetInfoAboutUser()
        {
            var user = _context.users.FirstOrDefault(x => x.UserId == UserId);

            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest("Op's! You have a problem!");
            }
        }
        
    }
}