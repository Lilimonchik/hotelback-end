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

        public RoomAction(ShopContext context,
            ILogger<TestAction> logger,
            IConfiguration config,
            IStorageService storageService)
        {
            _context = context;
            _logger = logger;
            _config = config;
            _storageService = storageService;
        }

        private Guid UserId => Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpPost("AddRoom")]
        public IActionResult AddRoom([FromForm] RoomModel argsAddRoom)
        {
            string Url = "";

            var user = _context.users
                .Where(x => x.UserId == UserId)
                .Include(c => c.CartIteams)
                .ThenInclude(y => y.Room)
                .FirstOrDefault();

            var categorymain = _context.categories
                .Where(x => x.CategoryName == argsAddRoom.Category)
                .FirstOrDefault();

            if (categorymain != null)
            {
                if (user != null)
                {
                    if (user.Role == Role.Admin)
                    {
                        var fileExt = Path.GetExtension(argsAddRoom.FileUrl.FileName);
                        string docName = Guid.NewGuid().ToString() + fileExt;

                        UploadFile(argsAddRoom.FileUrl,docName);

                        _context.rooms.Add(new Room
                        {
                            Price = argsAddRoom.Price,
                            CountOfPeople = argsAddRoom.CountPeople,
                            CountOfRoom = argsAddRoom.CountRoom,
                            RoomId = Guid.NewGuid(),
                            CategoryForId = categorymain.CategoryId,
                            Count = argsAddRoom.Count,
                            Discount = 0,
                            AccualFileUrl = docName
                        });

                        _context.SaveChanges();

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

                        _context.rooms.Add(new Room
                        {
                            Price = argsAddRoom.Price,
                            CountOfPeople = argsAddRoom.CountPeople,
                            CountOfRoom = argsAddRoom.CountRoom,
                            RoomId = Guid.NewGuid(),
                            Category = new Category
                            {
                                CategoryId = Guid.NewGuid(),
                                CategoryName = argsAddRoom.Category,
                            },
                            Count = argsAddRoom.Count,
                            Discount = 0,
                            AccualFileUrl = docName

                        });

                        _context.SaveChanges();

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
        public IActionResult ShowRoom()
        {
            List<RoomDTO> room = new List<RoomDTO>();

            _context.categories.Load();

            foreach (var rooms in _context.rooms)
            {
                room.Add(new RoomDTO
                {
                    CategoryName = rooms.Category.CategoryName,
                    Discount = rooms.Discount,
                    Price = rooms.Price,
                    RoomId = rooms.RoomId.ToString(),
                    CountOfPeople = rooms.CountOfPeople,
                    UrlPhoto = _config["UrlPhoto:url"]+rooms.AccualFileUrl
                }) ;
            }

            return Ok(room);
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