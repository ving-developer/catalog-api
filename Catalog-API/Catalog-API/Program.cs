using Catalog_API.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//getting the connection string from appsettings.json
var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConection");
//Configuring the DbContext to use the Entity Framework and be used for dependency injection
builder.Services.AddDbContext<CatalogApiContext>(options =>
                        options.UseMySql(mySqlConnection,
                        ServerVersion.AutoDetect(mySqlConnection)));


// Add services to the container.

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
