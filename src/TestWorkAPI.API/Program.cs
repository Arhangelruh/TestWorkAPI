using Microsoft.EntityFrameworkCore;
using TestWorkAPI.API.Interfaces;
using TestWorkAPI.API.Repository;
using TestWorkAPI.API.Services;
using TestWorkAPI.DB.Context;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("MyUsersDatabase");
builder.Services.AddDbContext<WorkAPIContext>(options =>
                options.UseSqlServer(connection));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped <IUserManager, UserManager> ();
builder.Services.AddScoped <IRoleManager, RoleManager>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
