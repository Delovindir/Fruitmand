using Fruitmand.Controllers;
using Fruitmand.Interfaces;
using Fruitmand.Models.Dto;
using Fruitmand.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FruitBasketApi.Tests
{
    public class FruitControllerTests
    {
        private readonly Mock<IFruitStorageService> _mockService;
        private readonly FruitController _controller;

        public FruitControllerTests()
        {
            _mockService = new Mock<IFruitStorageService>();
            _controller = new FruitController(_mockService.Object);
        }

        [Fact]
        public async Task GetOldest_ReturnsNotFound_WhenNoFruitExists()
        {
            _mockService.Setup(s => s.GetOldestFruitAsync("appel")).ReturnsAsync((FruitItem?)null);

            var result = await _controller.GetOldest("appel");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetOldest_ReturnsFruit_WhenExists()
        {
            var fruit = new FruitItem
            {
                SoortFruit = "appel",
                AankoopDatum = DateTime.UtcNow
            };

            _mockService.Setup(s => s.GetOldestFruitAsync("appel")).ReturnsAsync(fruit);

            var result = await _controller.GetOldest("appel") as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(fruit, result.Value);
        }

        [Fact]
        public async Task DeleteMand_CallsService()
        {
            _mockService.Setup(s => s.DeleteAllFruitsAsync()).Returns(Task.CompletedTask).Verifiable();

            var result = await _controller.DeleteMand();

            Assert.IsType<OkResult>(result);
            _mockService.Verify(s => s.DeleteAllFruitsAsync(), Times.Once);
        }
    }
}
