using System;
namespace Hotel.Context
{
	public class User
	{
		public Guid UserId { get; set; }

		public string Name { get; set; }

		public string Password { get; set; }

		public bool Online = false;

		public Role Role { get; set; }

		public ICollection<CartIteam> CartIteams { get; set; }
    }

	public enum Role
	{
		Admin,
		User
	}
}

