using AutoMapper;
using Catalog_API.AppServiceExtensions;
using Catalog_API.Context;
using Catalog_API.DTOs.Mappings;
using Catalog_API.Logging;
using Catalog_API.Repository;
using Catalog_API.Repository.Interfaces;
using Catalog_API.Services;
using Catalog_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;

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
builder.Services.AddControllers().AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler =
                System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Calls the extension methods on ServiceCollectionExtensions
builder.AddSwaggerApi()
       .AddAuthentication();

//Injects our pattern UnitOfWork which will be in charge of accessing the Repositories and persisting the information in the database
builder.Services.AddScoped<IUnityOfWork, UnityOfWork>();

//Create MapperConfiguration using MappingProfile to convert DTO objects in Models and vice versa
var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
//Adding mapper as singleton
builder.Services.AddSingleton(mappingConfig.CreateMapper());

//Adding our custom logs globally to the application
builder.Services.TryAddEnumerable(
           ServiceDescriptor.Singleton<ILoggerProvider, CustomLoggerProvider>());
LoggerProviderOptions.RegisterProviderOptions
    <CustomLoggerProviderConfiguration, CustomLoggerProvider>(builder.Services);

builder.Services.AddCors(opt => opt.AddPolicy("EnableApiGet", builder =>
                                builder.AllowAnyOrigin()
                                       .AllowAnyHeader()
                                       .AllowAnyMethod()));
//Add versioning
builder.Services.AddApiVersioning(opt =>
{
    opt.AssumeDefaultVersionWhenUnspecified = true;//tells the application to use the default API version when a version is not specified in the request
    opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1,0);//Informs the default API version
    opt.ReportApiVersions = true;//Informs in the Response of the request, information about the compatibility of the version used by the request
});
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";//Informs Swagger of the versioning pattern used in the endpoint url
    options.SubstituteApiVersionInUrl = true;
});
#endregion

var app = builder.Build();

#region Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerApi(app.Services.GetRequiredService<IApiVersionDescriptionProvider>());
}

app.UseHttpsRedirection();

app.UseCors();

//Use in "app" the authentication method added in "builder"
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
#endregion

app.Run();
