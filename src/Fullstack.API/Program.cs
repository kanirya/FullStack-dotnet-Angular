using Fullstack.API.Data;
using Fullstack.API.Models;
using Fullstack.Application.Services;
using Fullstack.Domain.Interfaces;
using Fullstack.Infrastructure.Options;
using Fullstack.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddIdentityApiEndpoints<AppUser>()
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit=false;
    options.Password.RequireUppercase=false;
    options.Password.RequireLowercase=false;
    options.User.RequireUniqueEmail=true;
});


// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.Configure<TmdbOptions>(
    builder.Configuration.GetSection("TMDbSettings"));

builder.Services.AddHttpClient<TmdbMovieRepository>();
builder.Services.AddScoped<IMovieRepository, TmdbMovieRepository>();
builder.Services.AddScoped<MovieService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // keeps original names
    });
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.MapGroup("/api")
    .MapIdentityApi<AppUser>();


app.MapPost("/api/signup",async (
    UserManager<AppUser> userManager,
    [FromBody] UserRegistrationModel userRegistrationModel
    ) => {
        AppUser user = new AppUser()
        {
            Email = userRegistrationModel.Email,
            FullName = userRegistrationModel.FullName,
            UserName = userRegistrationModel.Email 
        };
        var result=   await    userManager.
        CreateAsync(user, userRegistrationModel.Password);
        if (result.Succeeded)
            return Results.Ok(result);
        else
            return Results.BadRequest(result);
    });



app.Run();


public class UserRegistrationModel
{
    public string Email { set; get; }
    public string Password { set; get; }
    public string FullName { set; get; }
}