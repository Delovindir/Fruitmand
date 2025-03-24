using Fruitmand.Controllers;
using Fruitmand.Interfaces;
using Fruitmand.Models;
using Fruitmand.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FruitBasketApi.Tests
{
    public class FruitStorageServiceTests
    {
        private readonly Mock<IFruitStorageService> _mockService;

        public FruitStorageServiceTests()
        {
            _mockService = new Mock<IFruitStorageService>();
        }

        [Theory]
        [InlineData("banaan", null, 4)]
        [InlineData("appel", null, 8)]
        [InlineData("aardbei", null, 2)]
        [InlineData("kiwi", null, 5)]
        [InlineData("mango", null, 3)]
        public void GetHoudbaarheidInDagen_ReturnsExpected(string soort, string? appelType, int expected)
        {
            int result = FruitShelfLife.GetHoudbaarheidInDagen(soort, appelType);
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetMand_ReturnsCorrectSpoilage()
        {
            var fruits = new List<FruitItem>
            {
                new() { SoortFruit = "appel", AankoopDatum = DateTime.UtcNow.AddDays(-9) }, // spoiled
                new() { SoortFruit = "appel", AankoopDatum = DateTime.UtcNow.AddDays(-1) },
                new() { SoortFruit = "banaan", AankoopDatum = DateTime.UtcNow.AddDays(-5) }, // spoiled
                new() { SoortFruit = "kiwi", AankoopDatum = DateTime.UtcNow.AddDays(-1) }
            };

            _mockService.Setup(s => s.GetAllFruitsAsync()).ReturnsAsync(fruits);

            var controller = new FruitController(_mockService.Object);
            var result = await controller.GetMand() as OkObjectResult;

            Assert.NotNull(result);
            var dto = Assert.IsType<MandOverviewDto>(result.Value);

            Assert.Equal(2, dto.AantallenVanFruit["appel"]);
            Assert.True(dto.IsBedorven); // 2 out of 4 are spoiled
        }
    }
}
