using System;
using System.Security.Claims;
using Hotel.Context;
using Hotel.DataBase;
using Hotel.Models;
using Hotel.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    public class SignalRTest : Hub
    {
        public ShopContext _context;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _config;
        private readonly ILogger<TestAction> _logger;

        public SignalRTest(ShopContext context,
             ILogger<TestAction> logger,
            IConfiguration config,
            IStorageService storageService)
        {
            _context = context;
            _logger = logger;
            _config = config;
            _storageService = storageService;

        }

        /*public async Task AddRoom(Room room)
        {
            _context.Add(room);
            await Clients.All.SendAsync("RoomAdded", room);
        }
        */
        /*public async Task Echo(string message)
        {
            await Clients.All.SendAsync("Send",message);
        }*/

        public async Task UpdateRoomFilter(RoomDTO roomFilter)
        {
            await Clients.All.SendAsync("UpdateRoomFilter", roomFilter);
        }
    }
}