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
using Hotel.Services;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Hotel.Controllers
{

    [ApiController]
    [Route("Test")]
    public class TestAction : ControllerBase
    {
        private readonly IStorageService _storageService;
        private readonly IConfiguration _config;
        private readonly ILogger<TestAction> _logger;

        public TestAction(
        ILogger<TestAction> logger,
        IConfiguration config,
        IStorageService storageService)
        {
            _logger = logger;
            _config = config;
            _storageService = storageService;
        }

        [HttpPost("UploadFile")]
        public async Task<string> UploadFile(IFormFile file)
        {
            // Process file
            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var fileExt = Path.GetExtension(file.FileName);
            string docName = Guid.NewGuid().ToString() + fileExt;
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
        [HttpPost("SendFiles")]
        public IActionResult SendFiles()
        {
            string rootpath = Directory.GetCurrentDirectory() + "/Utility/Image";

            var files = Directory.GetFiles(rootpath,"", SearchOption.AllDirectories);

            return Ok(files);
        }
    }
}