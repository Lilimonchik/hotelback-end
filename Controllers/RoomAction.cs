using System;
using Hotel.Context;
using Hotel.DataBase;
using Hotel.Interfaces;
using Hotel.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Controllers
{
    [ApiController]
    [Route("RoomAction")]
	public class RoomAction
	{
		public MainInterface _context;

		public RoomAction(MainInterface context)
		{
			_context = context;
		}
        [HttpPost("AddRoom")]
        public void AddRoom(RoomModel argsAddRoom)
        {
            var user = _context.users.FirstOrDefault(x => x.UserId == argsAddRoom.UserId);

            if (user.Role == Role.Admin && user.Online == true)
            {
                _context.rooms.Add(new Room
                {
                    Price = argsAddRoom.Price,
                    CountOfPeople = argsAddRoom.CountPeople,
                    CountOfRoom = argsAddRoom.CountRoom,
                    IdRoom = Guid.NewGuid(),
                    Category = new Category
                    {
                        CategoryId = Guid.NewGuid(),
                        CategoryName = argsAddRoom.Category
                    },
                    Count = argsAddRoom.Count
                }); ;
                Console.WriteLine("Successful!");
            }
            else
            {
                Console.WriteLine("Op's! You're not an admin or not online!");
            }
        }
        [HttpPost("AddDiscount")]
        public string AddDiscount (DiscountModel discount)
        {
            var user = _context.users.FirstOrDefault(x => x.UserId == discount.UserId);

            var room = _context.rooms.FirstOrDefault(x => x.IdRoom == discount.RoomId);

            if(user != null && room != null)
            {
                if(user.Online && user.Role == Role.Admin)
                {
                    if (discount.TypeDiscount == "Interest" && ((discount.SumDiscount * room.Price) / 100) <= room.Price)
                    {
                        room.TypeDiscount = discount.TypeDiscount;

                        room.Discount = discount.SumDiscount;

                        room.Price -= ((room.Price * discount.SumDiscount) / 100);

                        return ("Successful!");
                    }
                    else if (discount.TypeDiscount == "Sum" && discount.SumDiscount <= room.Price)
                    {
                        room.Discount = discount.SumDiscount;

                        room.TypeDiscount = discount.TypeDiscount;

                        room.Price -= discount.SumDiscount;

                        return ("Successful!");
                    }
                }
            }
            return ("Op's! Incorrect data!");
        }
        [HttpGet("ShowRoom")]
        public void ShowRoom()
        {
            foreach(Room room in _context.rooms)
            {
                Console.WriteLine("Id: {0}  Type of room: {1}   Count of people: {2}  Price: {3}   Count of room: {4} Count: {5} Discount: {6} ", room.IdRoom, room.CountOfPeople, room.Price, room.CountOfRoom, room.Count,room.Discount);
                
            }
        }    
 
	}
}