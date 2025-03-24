using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Fruitmand.Controllers;
using Fruitmand.Models;
using Fruitmand.Services;
using Fruitmand.Interfaces;

namespace FruitBasketApi.Tests
{
    public class FruitControllerTests
    {
        [Fact]
        public async Task GetOldest_ReturnsNotFound_WhenNoFruitExists()
        {
            var mockService = new Mock<IFruitStorageService>(); mockService.Setup(s => s.GetOldestFruitAsync("appel")).ReturnsAsync((FruitItem?)null);
            var controller = new FruitController(mockService.Object);
            var result = await controller.GetOldest("appel");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetOldest_ReturnsFruit_WhenExists()
        {
            var mockService = new Mock<IFruitStorageService>(); var fruit = new FruitItem { SoortFruit = "appel", AankoopDatum = DateTime.UtcNow };
            mockService.Setup(s => s.GetOldestFruitAsync("appel")).ReturnsAsync(fruit);

            var controller = new FruitController(mockService.Object);
            var result = await controller.GetOldest("appel") as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(fruit, result.Value);
        }

        [Fact]
        public async Task DeleteMand_CallsService()
        {
            var mockService = new Mock<IFruitStorageService>();
            mockService.Setup(s => s.DeleteAllFruitsAsync()).Returns(Task.CompletedTask).Verifiable();

            var controller = new FruitController(mockService.Object);
            var result = await controller.DeleteMand();

            Assert.IsType<OkResult>(result);
            mockService.Verify(s => s.DeleteAllFruitsAsync(), Times.Once);
        }
    }
}