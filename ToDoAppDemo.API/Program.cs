using Microsoft.EntityFrameworkCore;
using ToDoAppDemo.API;
using ToDoAppDemo.API.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//This line of code essentially retrieves the connection string details
//stored in the application's configuration for a database connection named "DefaultConnection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//By calling AddDbContext, you're essentially registering the ToDoContext as a service that can be injected
//and used throughout your application. The UseSqlServer method configures this DbContext
//to use SQL Server as the underlying database technology.
builder.Services.AddDbContext<ToDoContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //These two lines of code, app.UseSwagger(); and app.UseSwaggerUI();,
    //are used together in ASP.NET Core applications to enable Swagger for API documentation
    app.UseSwagger();
    app.UseSwaggerUI();
}

//This line of code adds the HTTPS Redirection Middleware to the application's request pipeline. 
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//it will end the request,
//If you remove the app.Run() method from the Program.cs file
//in an ASP.NET Core application, the application will not start.
app.Run();
