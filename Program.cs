using Booking_System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BookingContext>(options => { options.UseSqlite("Filename=../../../MyLibrary.db"); });

builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateAudience = false,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "tyrkiskkamel.biz",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("etellerandetvirkeliglangtgerne256bit"))
    };
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    BookingContext context = scope.ServiceProvider.GetRequiredService<BookingContext>();
    context.Database.EnsureCreated();
}

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();