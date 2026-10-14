using backend.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly StoreContext _context;

        public BrandController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/Brand
        [HttpGet]
        public async Task<IEnumerable<BrandDto>> Get()
        {
            return await _context.Brands.Select(b => new BrandDto
            {
                Id = b.BrandId,
                Name = b.Name
            }).ToListAsync();
        }

        // POST: api/Brand
        [HttpPost]
        public async Task<ActionResult<BrandDto>> Add(BrandInsertDto brandInsertDto)
        {
            var brand = new Brand
            {
                Name = brandInsertDto.Name
            };

            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();

            var brandDto = new BrandDto
            {
                Id = brand.BrandId,
                Name = brand.Name
            };

            return CreatedAtAction(nameof(Get), new { id = brand.BrandId }, brandDto);
        }
    }
}
