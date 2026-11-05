using backend.DTOs;
using backend.services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeerController : ControllerBase
    {
        private  IValidator<BeerInsertDto> _beerInsertValidator;
        private  IValidator<BeerUpdateDto> _beerUpdateValidator;
        private  ICommonServices<BeerDto, BeerInsertDto, BeerUpdateDto> _beerService;

        public BeerController(
            IValidator<BeerInsertDto> beerInsertValidator,
            IValidator<BeerUpdateDto> beerUpdateValidator,
            [FromKeyedServices("beerService")]
           ICommonServices<BeerDto, BeerInsertDto, BeerUpdateDto> beerService)

        {
            _beerInsertValidator = beerInsertValidator;
            _beerUpdateValidator = beerUpdateValidator;
            _beerService = beerService;
        }

        // GET: api/Beer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BeerDto>>> Get()
        {
            return Ok(await _beerService.Get());
        }

        // GET: api/Beer/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BeerDto>> GetById(int id)
        {
            var beer = await _beerService.GetById(id);
            return beer == null ? NotFound() : Ok(beer);
        }

        // POST: api/Beer
        [HttpPost]
        public async Task<ActionResult<BeerDto>> Add(BeerInsertDto beerInsertDto)
        {
            var validationResult = await _beerInsertValidator.ValidateAsync(beerInsertDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors  .Select(e => new { e.PropertyName, e.ErrorMessage }));
            }

            var beerDto = await _beerService.Add(beerInsertDto);

            if (beerDto == null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetById), new { id = beerDto.Id }, beerDto);
        }



        // PUT: api/Beer/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<BeerDto>> Update(int id, BeerUpdateDto beerUpdateDto)
        {
            var validationResult = await _beerUpdateValidator.ValidateAsync(beerUpdateDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors
                    .Select(e => new { e.PropertyName, e.ErrorMessage }));
            }

            var beerDto = await _beerService.Update(id, beerUpdateDto);
            return beerDto == null ? NotFound() : Ok(beerDto);
        }

        // DELETE: api/Beer/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<BeerDto>> Delete(int id)
        {
            var beerDto = await _beerService.Delete(id);
            return beerDto == null ? NotFound() : Ok(beerDto);
        }
    }
}