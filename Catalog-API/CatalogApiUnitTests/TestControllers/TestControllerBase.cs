using AutoMapper;
using Catalog_API.DTOs.Mappings;
using Catalog_API.Repository;
using Catalog_API.Repository.Interfaces;
using CatalogApiUnitTests.Services;
using Xunit;

namespace CatalogApiUnitTests.TestControllers;

[Collection("DatabaseServiceCollection")]
public class TestControllerBase
{
    protected readonly IUnityOfWork _unity;
    protected readonly IMapper _mapper;

    public TestControllerBase(DatabaseService dbService)
    {
        var config = new MapperConfiguration(conf =>
        {
            conf.AddProfile(new MappingProfile());
        });

        _mapper = config.CreateMapper();
        _unity = new UnityOfWork(dbService.Context);
    }
}
