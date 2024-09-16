using JWT_API_BD.Models;
using Microsoft.EntityFrameworkCore;

using JWT_API_BD.Services.Interfaces;
using JWT_API_BD.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration.GetValue<string>("JwtSettings:Secret_Key");
var byteKey = Encoding.UTF8.GetBytes(key);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyecting custom services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Inyecting db context
builder.Services.AddDbContext<BasicUserAuthContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DB_String"));
});
// Ignoring cycle refences on foreign keys
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Adding JWT authentication
builder.Services.AddAuthentication(conf =>
{
    conf.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    conf.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer( conf => {
    conf.RequireHttpsMetadata = false;
    conf.SaveToken = true;
    conf.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(byteKey),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
