using Catalog_API.Context;
using Catalog_API.Repository;
using Catalog_API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Database connection
//getting the connection string from appsettings.json
var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConection");
//Configuring the DbContext to use the Entity Framework and be used for dependency injection
builder.Services.AddDbContext<CatalogApiContext>(options =>
                        options.UseMySql(mySqlConnection,
                        ServerVersion.AutoDetect(mySqlConnection)));
#endregion

#region Register services
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Injects our pattern UnitOfWork which will be in charge of accessing the Repositories and persisting the information in the database
builder.Services.AddScoped<IUnityOfWork, UnityOfWork>();
#endregion

var app = builder.Build();

#region Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
#endregion

app.Run();
