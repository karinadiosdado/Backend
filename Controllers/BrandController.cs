using backend.DTOs;
using backend.services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IValidator<BrandInsertDto> _brandInsertValidator;
        private readonly IValidator<BrandUpdateDto> _brandUpdateValidator;
        private readonly ICommonServices<BrandDto, BrandInsertDto, BrandUpdateDto> _brandService;

        public BrandController(
            IValidator<BrandInsertDto> brandInsertValidator,
            IValidator<BrandUpdateDto> brandUpdateValidator,
            [FromKeyedServices("brandService")]
            ICommonServices<BrandDto, BrandInsertDto, BrandUpdateDto> brandService)
        {
            _brandInsertValidator = brandInsertValidator;
            _brandUpdateValidator = brandUpdateValidator;
            _brandService = brandService;
        }

        // GET: api/Brand
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandDto>>> Get()
        {
            return Ok(await _brandService.Get());
        }

        // GET: api/Brand/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BrandDto>> GetById(int id)
        {
            var brand = await _brandService.GetById(id);
            return brand == null ? NotFound() : Ok(brand);
        }

        // POST: api/Brand
        [HttpPost]
        public async Task<ActionResult<BrandDto>> Add(BrandInsertDto brandInsertDto)
        {
            var validationResult = await _brandInsertValidator.ValidateAsync(brandInsertDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }

            var brandDto = await _brandService.Add(brandInsertDto);

            if (brandDto == null)
            {
                return Conflict(new { message = "Ya existe una marca con ese nombre." });
            }

            return CreatedAtAction(nameof(GetById), new { id = brandDto.Id }, brandDto);
        }

        // PUT: api/Brand/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<BrandDto>> Update(int id, BrandUpdateDto brandUpdateDto)
        {
            var validationResult = await _brandUpdateValidator.ValidateAsync(brandUpdateDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }

            var existingBrand = await _brandService.GetById(id);
            if (existingBrand == null)
            {
                return NotFound();
            }

            var brandDto = await _brandService.Update(id, brandUpdateDto);

            if (brandDto == null)
            {
                return Conflict(new { message = "Ya existe otra marca con ese nombre." });
            }

            return Ok(brandDto);
        }

        // DELETE: api/Brand/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<BrandDto>> Delete(int id)
        {
            var existingBrand = await _brandService.GetById(id);

            if (existingBrand == null)
            {
                return NotFound();
            }

            //  Impedir eliminar una marca que tenga cervezas asociadas (409 Conflict)
            if (_brandService is BrandService service && await service.HasBeers(id))
            {
                return Conflict(new { message = "No se puede eliminar la marca porque tiene cervezas asociadas." });
            }

            var brandDto = await _brandService.Delete(id);
            return Ok(brandDto);
        }

        // GET: api/Brand/{id}/beers
        //   devuelve las cervezas de esa marca
        [HttpGet("{id}/beers")]
        public async Task<ActionResult<IEnumerable<BeerDto>>> GetBeersByBrand(int id)
        {
            if (_brandService is BrandService service)
            {
                var beers = await service.GetBeersByBrandId(id);

                if (beers == null)
                {
                    return NotFound(new { message = $"No se encontró la marca con Id {id}." });
                }

                return Ok(beers);
            }

            return StatusCode(500, "Servicio de marcas no compatible.");
        }
    }
}
