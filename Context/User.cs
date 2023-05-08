using System;
namespace Hotel.Context
{
	public class User
	{
		public Guid UserId { get; set; }

		public string FirstName { get; set; }

		public string Name { get; set; }

		public string Email { get; set; }

		public DateTime Birthday { get; set; }

		public string UserName { get; set; }

		public string Password { get; set; }

		public Role Role { get; set; }

		public ICollection<Order> Orders { get; set; }

		public ICollection<CartIteam> CartIteams { get; set; }
    }

	public enum Role
	{
		Admin,
		User
	}
}

