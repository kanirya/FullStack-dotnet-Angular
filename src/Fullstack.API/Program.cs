using Fullstack.API.Data;
using Fullstack.API.Models;
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