using System;
using Hotel.Context;
using Hotel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Hotel.DataBase
{
	public class ShopContext : DbContext
	{
		public ShopContext(DbContextOptions<ShopContext> options) : base(options)
		{

		}

		public DbSet<User> users { get; set; }

		public DbSet<Room> rooms { get; set; }

		public DbSet<Category> categories { get; set; }

		public DbSet<CartIteam> cartIteams { get; set; }

		public DbSet<Promocode> promocodes { get; set; }

		public DbSet<OrderIteam> orderIteams { get; set; }

		public DbSet<Order> orders { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Order>()
				.HasOne(x => x.User)
				.WithMany(y => y.Orders)
				.HasForeignKey(c => c.UserId);

			modelBuilder.Entity<CartIteam>()
				.HasOne(x => x.User)
				.WithMany(y => y.CartIteams)
				.HasForeignKey(c => c.UserId);

			modelBuilder.Entity<CartIteam>()
				.HasOne(x => x.Room)
				.WithMany(y => y.CartIteams)
				.HasForeignKey(c => c.RoomId);

			modelBuilder.Entity<OrderIteam>()
				.HasOne(x => x.Order)
				.WithMany(y => y.OrderIteams)
				.HasForeignKey(c => c.OrderId);

            modelBuilder.Entity<Room>()
                .HasOne(x => x.Category)
                .WithMany(y => y.Rooms)
                .HasForeignKey(c => c.CategoryForId);

            modelBuilder.Entity<OrderIteam>()
				.HasOne(x => x.Room)
				.WithMany(y => y.OrderIteams)
				.HasForeignKey(c => c.RoomId);

            modelBuilder.Entity<User>().HasKey(s => new { s.UserId });

            modelBuilder.Entity<Room>().HasKey(s => new { s.RoomId });

            modelBuilder.Entity<Category>().HasKey(s => new { s.CategoryId });

            modelBuilder.Entity<CartIteam>().HasKey(s => new { s.CartIteamId});

            modelBuilder.Entity<Promocode>().HasKey(s => new { s.PromocodeId });

            modelBuilder.Entity<OrderIteam>().HasKey(s => new { s.OrderIteamId });

            modelBuilder.Entity<Order>().HasKey(s => new { s.OrderId });
        }
	}
}

