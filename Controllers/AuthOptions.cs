using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
namespace Hotel.Controllers
{
	public class AuthOptions
	{
		public string Issuer { get; set; }

		public string Audience { get; set; }

		public string Secret { get; set; }

		public int TokenLifeTime { get; set; }

		public SymmetricSecurityKey GetSymmetricSecutityKey()
		{
			return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));
		}
	}
}

