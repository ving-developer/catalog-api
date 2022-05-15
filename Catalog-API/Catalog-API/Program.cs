using Catalog_API.AppServiceExtensions;
using Catalog_API.Context;
using Catalog_API.Repository;
using Catalog_API.Repository.Interfaces;
using Catalog_API.Services;
using Catalog_API.Services.Interfaces;
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
//Adding the JWT Token Generator to be used by Dependency Injection
builder.Services.AddSingleton<ITokenService>(new TokenService());
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Calls the extension methods on ServiceCollectionExtensions
builder.AddSwaggerApi()
       .AddAuthentication();

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

//Use in "app" the authentication method added in "builder"
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
#endregion

app.Run();
