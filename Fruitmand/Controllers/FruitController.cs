using Fruitmand.Interfaces;
using Fruitmand.Models;
using Fruitmand.Models.Dto;
using Fruitmand.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Fruitmand.Controllers
{
    [ApiController]
    [Route("fruit")]
    public class FruitController : ControllerBase
    {
        private readonly IFruitStorageService _storage;

        public FruitController(IFruitStorageService storage)
        {
            _storage = storage;
        }

        [HttpPut("{soort}")]
        public async Task<IActionResult> AddFruit(string soort, [FromBody] AddFruitRequest body)
        {
            var fruit = new FruitItem
            {
                SoortFruit = soort,
                AankoopDatum = body.AankoopDatum,
                IsBiologisch = body.IsBiologisch,
                SoortAppel = body.SoortAppel
            };

            await _storage.AddFruitAsync(soort, fruit);
            return Ok();
        }

        [HttpGet("{soort}")]
        public async Task<IActionResult> GetOldest(string soort)
        {
            var fruit = await _storage.GetOldestFruitAsync(soort);
            if (fruit == null) return NotFound();
            return Ok(fruit);
        }

        [HttpDelete("{soort}")]
        public async Task<IActionResult> DeleteOldest(string soort)
        {
            await _storage.DeleteOldestFruitAsync(soort);
            return Ok();
        }

        [HttpGet("/mand")]
        public async Task<IActionResult> GetMand()
        {
            var allFruits = await _storage.GetAllFruitsAsync();
            var grouped = allFruits.GroupBy(f => f.SoortFruit)
                                   .ToDictionary(g => g.Key, g => g.Count());

            var percentageBedorven = allFruits.Count == 0
                ? 0
                : allFruits.Count(f => f.IsBedorven) * 100 / allFruits.Count;

            var dto = new MandOverviewDto
            {
                AantallenVanFruit = grouped,
                IsBedorven = percentageBedorven > 20
            };

            return Ok(dto);
        }

        [HttpDelete("/mand")]
        public async Task<IActionResult> DeleteMand()
        {
            await _storage.DeleteAllFruitsAsync();
            return Ok();
        }

        [HttpGet("/mand/detail")]
        public async Task<IActionResult> GetMandDetail()
        {
            var allFruits = await _storage.GetAllFruitsAsync();
            var aantal = allFruits.Count;
            var bedorven = allFruits.Count(f => f.IsBedorven);

            var dto = new MandDetailDto
            {
                Fruits = allFruits,
                TotaalAantal = aantal,
                AantalBedorven = bedorven,
                SpoilagePercentage = aantal == 0 ? 0 : (int)Math.Round((double)bedorven * 100 / aantal)
            };

            return Ok(dto);
        }
    }
}
