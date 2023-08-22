using KH.Lab.WebAPIJWT.Controllers;
using KH.Lab.WebAPIJWT.Data;
using KH.Lab.WebAPIJWT.Models;
using KH.Lab.WebAPIJWT.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace KH.Lab.WebAPIJWT.Test
{
    public class ProductControllerTest
    {
        ProductUnitTestController _controller;
        IProductService _service;

        public ProductControllerTest()
        {
            var mockConfSection = new Mock<IConfigurationSection>();
            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "DefaultConnection")]).Returns("Server=(localdb)\\mssqllocaldb;Database=JWT;Trusted_Connection=True;MultipleActiveResultSets=true;");

            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(a => a.GetSection(It.Is<string>(s => s == "ConnectionStrings"))).Returns(mockConfSection.Object);

            var _dbContext = new DbContextClass(mockConfiguration.Object);

            _service = new ProductService(_dbContext);
            _controller = new ProductUnitTestController(_service);
        }

        [Fact]
        public void GetProductList_ProductList()
        {
            // Arrange

            // Act
            var result = _controller.GetAllProducts();
            var resultType = result as OkObjectResult;
            var resultList = resultType.Value as List<Product>;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Product>>(resultType.Value);
            Assert.Equal(2, resultList.Count);
        }
    }
}
