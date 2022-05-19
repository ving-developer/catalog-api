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

public class ProductsControllerUnitTest : TestControllerBase
{
    private readonly ProductsController _controller;

    public ProductsControllerUnitTest(DatabaseService dbService) : base(dbService)
    {
        _controller = new ProductsController(_unity, _mapper);
    }

    [Fact]
    public async Task GetAsync_MatchResult()
    {
        //Arrange
        var parameters = new ProductParameters { PageNumber = 2, PageSize = 2 };

        //Act
        var response = await _controller.GetAsync(parameters);

        //Assert
        var products = response.Value.Should().BeAssignableTo<List<ProductDTO>>().Subject;
        var productThree = _mapper.Map<ProductDTO>(Constants.ProductThree);
        var productFour = _mapper.Map<ProductDTO>(Constants.ProductFour);
        productThree.Name.Should().BeEquivalentTo(products[0].Name);
        productFour.Name.Should().BeEquivalentTo(products[1].Name);
    }

    [Fact]
    public async Task GetByIdAsync_MatchResult()
    {
        //Act
        var response = await _controller.GetByIdAsync(1);

        //Assert
        var productOne = _mapper.Map<ProductDTO>(Constants.ProductOne);
        productOne.Name.Should().BeEquivalentTo(response.Value.Name);
    }

    [Fact]
    public async Task PostAsync_Return_CreatedResult()
    {
        //Arrange
        var newProduct = new ProductDTO
        {
            Name = "Product BY POST",
            Description = "Same Description",
            Price = 11.5f,
            ImageUrl = "product-post.jpg",
            CategoryId = 1
        };

        //Act
        var response = await _controller.PostAsync(newProduct);

        //Assert
        Assert.IsType<CreatedAtRouteResult>(response);
    }

    [Fact]
    public async Task PutAsync_Return_NoContent()
    {
        //Arrange
        var existingProduct = Constants.ProductFive;

        //Act
        var updatedProduct =
            new ProductDTO
            {
                ProductId = existingProduct.ProductId,
                Name = existingProduct.Name + " UPDATED BY PUT",
                Description = existingProduct.Description + " UPDATED BY PUT",
                Price = existingProduct.Price,
                ImageUrl = existingProduct.ImageUrl,
                CategoryId = existingProduct.CategoryId
            };
        var response = await _controller.PutAsync(existingProduct.ProductId, updatedProduct);

        //Assert
        Assert.IsType<NoContentResult>(response.Result);
    }

    [Fact]
    public async Task PutAsync_Return_BadRequest()
    {
        //Arrange
        var existingProduct = Constants.ProductOne;

        //Act
        var updatedProduct = new ProductDTO();
        var response = await _controller.PutAsync(existingProduct.CategoryId, updatedProduct);

        //Assert
        Assert.IsType<BadRequestResult>(response.Result);
    }

    [Fact]
    public async Task DeleteAsync_Return_OkResult()
    {
        //Act
        var response = await _controller.DeleteAsync(Constants.ProductSix.ProductId);

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
