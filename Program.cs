using Hotel.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Hotel.Controllers;
using Hotel.Context;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore.SqlServer;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.Extensions.FileProviders;
using Hotel.Services;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllCors", builder =>
    {
        builder
        .WithOrigins()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .SetIsOriginAllowed(delegate (string requestingOrigin)
        {
            return true;
        }).Build();
    });
});

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSignalR();

builder.Services.AddScoped<IStorageService, StorageService>();

var authOptionsConfiguration = builder.Configuration.GetSection("Auth");
builder.Services.Configure<AuthOptions>(authOptionsConfiguration);
var authOptions = builder.Configuration.GetSection("Auth").Get<AuthOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authOptions.Issuer,

            ValidateAudience = true,
            ValidAudience = authOptions.Audience,

            ValidateLifetime = true,

            IssuerSigningKey = authOptions.GetSymmetricSecutityKey(),
            ValidateIssuerSigningKey = true,


        };
    }
    );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
/*builder.Services.AddDbContext<ShopContext>(option =>
{
    option.UseSqlServer("Server=tcp:server-for-project.database.windows.net,1433;Initial Catalog=sushi-dataBase;Persist Security Info=False;User ID=oleh;Password=QWUngoSdd13Ss@123@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
});*/
builder.Services.AddDbContext<ShopContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
}, ServiceLifetime.Transient);
//builder.Services.AddDbContext<ShopContext>(options =>
//              options.UseSqlServer("Server = hotel-db.c0mjcdzvqqvd.us-east-1.rds.amazonaws.com, 3306; Initial Catalog = Hotelinfo; Persist Security Info=False; User ID = admin; Password = andrew12; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30; "));
var app = builder.Build();
//builder.Services.AddDbContext<ShopContext>(options =>
//           options.UseSqlServer("Server=tcp:anular-shop.database.windows.net,1433;Initial Catalog=project-database;Persist Security Info=False;User ID=oleg;Password=QWUngoSdd13Ss@123@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
//builder.Services.AddControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();

app.UseCors("AllowAllCors");

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.MapHub<SignalRTest>("/signalRTest");

app.UseAuthorization(); 

app.MapControllers();

app.Run();

