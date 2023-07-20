using Microsoft.EntityFrameworkCore;
using ToDoList.Business.Services;
using ToDoList.Business.Validations;
using ToDoList.Data;
using ToDoList.Data.Constants;
using ToDoList.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Register ToDoListDbContext as a service
builder.Services.AddDbContext<ToDoListDbContext>(options =>
    options.UseSqlServer(Consts.dbConnectionString));

// Add services to the container.
builder.Services.AddScoped<IToDoListService, ToDoListService>();
builder.Services.AddScoped<IToDoListRepository, ToDoListRepository>();
builder.Services.AddScoped<Validations>();

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
