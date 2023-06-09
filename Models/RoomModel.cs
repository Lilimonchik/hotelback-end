﻿using System;
namespace Hotel.Models
{
	public class RoomModel
	{
        public int Price { get; set; }

        public int CountPeople { get; set; }

        public int CountRoom { get; set; }

        public int Count { get; set; }

        public string About { get; set; }

        public Guid UserId { get; set; }

        public string Category { get; set; }

        public IFormFile FileUrl { get; set; }
    }
}

