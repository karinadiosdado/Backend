using backend.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeerController : ControllerBase
    {
        public StoreContext _Context;

        public BeerController(StoreContext context)
        {
            _Context = context;
        }

        // GET: api/Beer
        [HttpGet]
        public async Task<IEnumerable<BeerDto>> Get()
        {
            return await _Context.Beers.Select(b => new BeerDto
            {
                Id = b.BeerId,
                Name = b.Name,
                Alcohol = b.Alcohol,
                BrandId = b.BrandId
            }).ToListAsync();
        }

        // GET: api/Beer/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BeerDto>> GetById(int id)
        {
            var beer = await _Context.Beers.FindAsync(id);

            if (beer == null)
            {
                return NotFound();
            }

            var beerDto = new BeerDto
            {
                Id = beer.BeerId,
                Name = beer.Name,
                Alcohol = beer.Alcohol,
                BrandId = beer.BrandId
            };

            return Ok(beerDto);
        }

        // POST: api/Beer
        [HttpPost]
        public async Task<ActionResult<BeerDto>> Add(BeerInsertDto beerInsertDto)
        {
            var beer = new Beer
            {
                Name = beerInsertDto.Name,
                BrandId = beerInsertDto.BrandId,
                Alcohol = beerInsertDto.Alcohol
            };

            await _Context.Beers.AddAsync(beer);
            await _Context.SaveChangesAsync();

            var beerDto = new BeerDto
            {
                Id = beer.BeerId,
                Name = beer.Name,
                Alcohol = beer.Alcohol,
                BrandId = beer.BrandId
            };

            return CreatedAtAction(nameof(GetById), new { id = beer.BeerId }, beerDto);
        }

        // PUT: api/Beer/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BeerUpdateDto beerUpdateDto)
        {
            var beer = await _Context.Beers.FindAsync(id);
            if (beer == null)
            {
                return NotFound();
            }

            beer.Name = beerUpdateDto.Name;
            beer.BrandId = beerUpdateDto.BrandId;
            beer.Alcohol = beerUpdateDto.Alcohol;

            await _Context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Beer/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var beer = await _Context.Beers.FindAsync(id);
            if (beer == null)
            {
                return NotFound();
            }

            _Context.Beers.Remove(beer);
            await _Context.SaveChangesAsync();

            return NoContent();
        }
    }
}
