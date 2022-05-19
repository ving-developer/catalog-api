using Catalog_API.Models;
using System;

namespace CatalogApiUnitTests;

static class Constants
{
    #region Categories
    public static readonly Category CategoryOne =
        new Category { CategoryId = 1, Name = "Category 1", ImageUrl = "category1.jpg" };

    public static readonly Category CategoryTwo =
        new Category { CategoryId = 2, Name = "Category 2", ImageUrl = "category2.jpg" };

    public static readonly Category CategoryThree =
        new Category { CategoryId = 3, Name = "Category 3", ImageUrl = "category3.jpg" };

    public static readonly Category CategoryFour =
        new Category { CategoryId = 4, Name = "Category 4", ImageUrl = "category4.jpg" };
    #endregion

    #region Products
    public static readonly Product ProductOne =
        new Product
        {
            ProductId = 1,
            Name = "Product 1",
            Description = "Same Description",
            Price = 10.5f,
            ImageUrl = "product1.jpg",
            Stock = 10,
            RegisterDate = DateTime.Now,
            CategoryId = 1
        };

    public static readonly Product ProductTwo =
        new Product
        {
            ProductId = 2,
            Name = "Product 2",
            Description = "Same Description",
            Price = 11.5f,
            ImageUrl = "product2.jpg",
            Stock = 11,
            RegisterDate = DateTime.Now,
            CategoryId = 1
        };

    public static readonly Product ProductThree =
        new Product
        {
            ProductId = 3,
            Name = "Product 3",
            Description = "Same Description",
            Price = 12.5f,
            ImageUrl = "product3.jpg",
            Stock = 12,
            RegisterDate = DateTime.Now,
            CategoryId = 1,
            Category = CategoryOne,
        };

    public static readonly Product ProductFour =
        new Product
        {
            ProductId = 4,
            Name = "Product 4",
            Description = "Same Description",
            Price = 13.5f,
            ImageUrl = "product4.jpg",
            Stock = 13,
            RegisterDate = DateTime.Now,
            CategoryId = 2,
            Category = CategoryTwo,
        };

    public static readonly Product ProductFive =
        new Product
        {
            ProductId = 5,
            Name = "Product 5",
            Description = "Same Description",
            Price = 14.5f,
            ImageUrl = "product5.jpg",
            Stock = 14,
            RegisterDate = DateTime.Now,
            CategoryId = 3,
            Category = CategoryThree,
        };

    public static readonly Product ProductSix =
        new Product
        {
            ProductId = 6,
            Name = "Product 6",
            Description = "Same Description",
            Price = 14.5f,
            ImageUrl = "product6.jpg",
            Stock = 14,
            RegisterDate = DateTime.Now,
            CategoryId = 3,
            Category = CategoryThree,
        };
    #endregion
}
