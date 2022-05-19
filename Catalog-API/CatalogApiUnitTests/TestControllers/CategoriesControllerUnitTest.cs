using Catalog_API.Controllers.V1;
using Catalog_API.DTOs;
using Catalog_API.Pagination;
using CatalogApiUnitTests.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CatalogApiUnitTests.TestControllers;

public class CategoriesControllerUnitTest : TestControllerBase
{
    private readonly CategoriesController _controller;
    public CategoriesControllerUnitTest(DatabaseService dbService) : base(dbService)
    {
        _controller = new CategoriesController(_unity, _mapper);
    }

    [Fact]
    public async Task GetAsync_MatchResult()
    {
        //Arrange
        var parameters = new CategoryParameters { PageNumber = 1, PageSize = 2 };

        //Act
        var response = await _controller.GetAsync(parameters);

        //Assert
        var categories = response.Value.Should().BeAssignableTo<List<CategoryDTO>>().Subject;
        var categoryOne = _mapper.Map<CategoryDTO>(Constants.CategoryOne);
        var categoryTwo = _mapper.Map<CategoryDTO>(Constants.CategoryTwo);
        categoryOne.Name.Should().BeEquivalentTo(categories[0].Name);
        categoryTwo.Name.Should().BeEquivalentTo(categories[1].Name);
    }

    [Fact]
    public async Task GetByIdAsync_MatchResult()
    {
        //Act
        var response = await _controller.GetByIdAsync(1);

        //Assert
        var categoryOne = _mapper.Map<CategoryDTO>(Constants.CategoryOne);
        categoryOne.Name.Should().BeEquivalentTo(response.Value.Name);
    }

    [Fact]
    public async Task GetByIdAsync_Return_NotFound()
    {
        //Act
        var response = await _controller.GetByIdAsync(-1);

        //Assert
        Assert.IsType<NotFoundResult>(response.Result);
    }

    [Fact]
    public async Task PostAsync_Return_CreatedResult()
    {
        //Arrange
        var newCategory = new CategoryDTO { Name = "Mock Category", ImageUrl = "mock.jpg"};

        //Act
        var response = await _controller.PostAsync(newCategory);

        //Assert
        Assert.IsType<CreatedAtRouteResult>(response.Result);
    }

    [Fact]
    public async Task PutAsync_Return_NoContent()
    {
        //Arrange
        var existingCategory = Constants.CategoryFour;

        //Act
        var updatedCategory =
            new CategoryDTO
            {
                CategoryId = existingCategory.CategoryId,
                Name = existingCategory.Name + " Updated by PUT",
                ImageUrl = existingCategory.ImageUrl
            };
        var response = await _controller.PutAsync(existingCategory.CategoryId, updatedCategory);

        //Assert
        Assert.IsType<NoContentResult>(response.Result);
    }

    [Fact]
    public async Task PutAsync_Return_BadRequest()
    {
        //Arrange
        var existingCategory = Constants.CategoryThree;

        //Act
        var updatedCategory = new CategoryDTO();
        var response = await _controller.PutAsync(existingCategory.CategoryId, updatedCategory);

        //Assert
        Assert.IsType<BadRequestResult>(response.Result);
    }

    [Fact]
    public async Task DeleteAsync_Return_OkResult()
    {
        //Act
        var response = await _controller.DeleteAsync(Constants.CategoryThree.CategoryId);

        //Assert
        Assert.IsType<OkObjectResult>(response.Result);
    }

    [Fact]
    public async Task DeleteAsync_Return_NotFound()
    {
        //Act
        var response = await _controller.DeleteAsync(-1);

        //Assert
        Assert.IsType<NotFoundResult>(response.Result);
    }
}
