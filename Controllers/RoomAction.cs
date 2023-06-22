using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using Hotel.Context;
using Hotel.DataBase;
using Hotel.Interfaces;
using Hotel.Models;
using Hotel.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;

namespace Hotel.Controllers
{
    [ApiController]
    [Route("RoomAction")]
    public class RoomAction : ControllerBase
    {
        public ShopContext _context;

        private readonly IStorageService _storageService;
        private readonly IConfiguration _config;
        private readonly ILogger<TestAction> _logger;
        private readonly IHubContext<SignalRTest> _hubContext;

        public RoomAction(ShopContext context,
            ILogger<TestAction> logger,
            IConfiguration config,
            IStorageService storageService,
            IHubContext<SignalRTest> hubContext
            )
        {
            _context = context;
            _logger = logger;
            _config = config;
            _storageService = storageService;
            _hubContext = hubContext;
        }

        private Guid UserId => Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpPost("AddRoom")]
        public async Task<IActionResult> AddRoom([FromForm] RoomModel argsAddRoom)
        {
            string Url = "";

            var user = await _context.users
                .Where(x => x.UserId == UserId)
                .Include(c => c.CartIteams)
                .ThenInclude(y => y.Room)
                .FirstOrDefaultAsync();

            var categorymain = await _context.categories
                .Where(x => x.CategoryName == argsAddRoom.Category)
                .FirstOrDefaultAsync();

            if (categorymain != null)
            {
                if (user != null)
                {
                    if (user.Role == Role.Admin)
                    {
                        var fileExt = Path.GetExtension(argsAddRoom.FileUrl.FileName);
                        string docName = Guid.NewGuid().ToString() + fileExt;

                        UploadFile(argsAddRoom.FileUrl,docName);

                        Guid RoomId = Guid.NewGuid();

                        _context.rooms.Add(new Room
                        {
                            Price = argsAddRoom.Price,
                            CountOfPeople = argsAddRoom.CountPeople,
                            CountOfRoom = argsAddRoom.CountRoom,
                            RoomId = RoomId,
                            CategoryForId = categorymain.CategoryId,
                            Count = argsAddRoom.Count,
                            Discount = 0,
                            AccualFileUrl = docName
                        });

                        await _context.SaveChangesAsync();
                        await _hubContext.Clients.All.SendAsync("UpdateRoomFilter", new RoomDTO
                        {
                            CategoryName = argsAddRoom.Category,
                            Discount = 0,
                            Price = argsAddRoom.Price,
                            RoomId = RoomId.ToString(),
                            CountOfPeople = argsAddRoom.CountPeople,
                            UrlPhoto = _config["UrlPhoto:url"] + docName
                        });

                        return Ok("Successful!");
                    }
                    else
                    {
                        return BadRequest("Op's! You're not an admin or not online!");
                    }
                }
                else
                {
                    return BadRequest("Op's! Error!");
                }
            }
            else
            {
                if (user != null)
                {
                    if (user.Role == Role.Admin)
                    {
                        var fileExt = Path.GetExtension(argsAddRoom.FileUrl.FileName);
                        string docName = Guid.NewGuid().ToString() + fileExt;
                        UploadFile(argsAddRoom.FileUrl,docName);

                        Guid CategoryId = Guid.NewGuid();

                        Guid RoomId = Guid.NewGuid();

                        _context.rooms.Add(new Room
                        {
                            Price = argsAddRoom.Price,
                            CountOfPeople = argsAddRoom.CountPeople,
                            CountOfRoom = argsAddRoom.CountRoom,
                            RoomId = RoomId,
                            Category = new Category
                            {
                                CategoryId = CategoryId,
                                CategoryName = argsAddRoom.Category,
                            },
                            Count = argsAddRoom.Count,
                            Discount = 0,
                            AccualFileUrl = docName

                        });

                        await _context.SaveChangesAsync();
                        await _hubContext.Clients.All.SendAsync("UpdateRoomFilter", new RoomDTO
                        {
                            CategoryName = argsAddRoom.Category,
                            Discount = 0,
                            Price = argsAddRoom.Price,
                            RoomId = RoomId.ToString(),
                            CountOfPeople = argsAddRoom.CountPeople,
                            UrlPhoto = _config["UrlPhoto:url"] + docName
                        });
                        return Ok("Successful!");
                    }
                    else
                    {
                        return BadRequest("Op's! You're not an admin or not online!");
                    }
                }
                else
                {
                    return BadRequest("Op's! Error!");
                }
            }
            return Ok();
        }

        [HttpPost("AddDiscount")]
        public IActionResult AddDiscount(DiscountModel discount)
        {
            var user = _context.users.FirstOrDefault(x => x.UserId == UserId);

            var room = _context.rooms.FirstOrDefault(x => x.RoomId == discount.RoomId);

            if (user != null && room != null)
            {
                if (user.Role == Role.Admin)
                {
                    if (discount.TypeDiscount == "Interest" && ((discount.SumDiscount * room.Price) / 100) <= room.Price)
                    {
                        room.Discount = discount.SumDiscount;

                        room.Price -= ((room.Price * discount.SumDiscount) / 100);

                        _context.SaveChanges();

                        return Ok("Successful!");
                    }
                    else if (discount.TypeDiscount == "Sum" && discount.SumDiscount <= room.Price)
                    {
                        room.Discount = discount.SumDiscount;

                        room.Price -= discount.SumDiscount;

                        _context.SaveChanges();

                        return Ok("Successful!");
                    }
                    else
                    {
                        return BadRequest("Op's! Error!");
                    }
                }
            }
            return BadRequest("Op's! Incorrect data!");
        }

        [HttpGet("ShowRoom")]
        public async Task<IActionResult> ShowRoom()
        {
            List<RoomDTO> rooms = new List<RoomDTO>();

            await _context.categories.LoadAsync();

            foreach (var context_rooms in _context.rooms)
            {
                rooms.Add(new RoomDTO
                {
                    CategoryName = context_rooms.Category.CategoryName,
                    Discount = context_rooms.Discount,
                    Price = context_rooms.Price,
                    RoomId = context_rooms.RoomId.ToString(),
                    CountOfPeople = context_rooms.CountOfPeople,
                    UrlPhoto = _config["UrlPhoto:url"]+ context_rooms.AccualFileUrl
                }) ;
            }
            await _hubContext.Clients.All.SendAsync("askServerResponse", rooms);

            return Ok(rooms);
            
        }

        [HttpGet("ShowRoomForId")]
        public async Task<IActionResult> ShowRoomForId(string roomId)
        {
            var room = await _context.rooms.FirstOrDefaultAsync(x => x.RoomId.ToString() == roomId);

            await _context.categories.LoadAsync();

            var send_room = new RoomDTO
            {
                CategoryName = room.Category.CategoryName,
                Discount = room.Discount,
                Price = room.Price,
                RoomId = room.RoomId.ToString(),
                CountOfPeople = room.CountOfPeople,
                UrlPhoto = _config["UrlPhoto:url"] + room.AccualFileUrl
            };
            return Ok(send_room);
        }

        [HttpPost("UploadFile")]
        public async Task<string> UploadFile(IFormFile file,string docName)
        {
            // Process file
            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            //var docName = $"{Guid.NewGuid}";
            // call server

            var s3Obj = new S3Object()
            {
                BucketName = "hotel-image-andrew",
                InputStream = memoryStream,
                Name = docName
            };

            var cred = new AwsCredentials()
            {
                AccessKey = _config["AwsConfiguration:AWSAccessKey"],
                SecretKey = _config["AwsConfiguration:AWSSecretKey"]
            };

            var result = await _storageService.UploadFileAsync(s3Obj, cred);
            // 
            return (docName);

        }
    }
}