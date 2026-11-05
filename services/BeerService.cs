using backend.DTOs;
using backend.Models;
using backend.Repository;

namespace backend.services;

public class BeerService : ICommonServices<BeerDto, BeerInsertDto, BeerUpdateDto>
{
    private readonly IRepository<Beer> _beerRepository;

    public BeerService(IRepository<Beer> beerRepository)
    {
        _beerRepository = beerRepository;
    }

    public async Task<IEnumerable<BeerDto>> Get()
    {
        var beers = await _beerRepository.Get();
        return beers.Select(b => new BeerDto
        {
            Id = b.BeerId,
            Name = b.Name,
            Alcohol = b.Alcohol,
            BrandId = b.BrandId
        });
    }

    public async Task<BeerDto> GetById(int id)
    {
        var beer = await _beerRepository.GetById(id);

        if (beer == null)
        {
            return null;
        }

        return new BeerDto
        {
            Id = beer.BeerId,
            Name = beer.Name,
            Alcohol = beer.Alcohol,
            BrandId = beer.BrandId
        };
    }

    public async Task<BeerDto> Add(BeerInsertDto beerInsert)
    {
        var beer = new Beer
        {
            Name = beerInsert.Name,
            BrandId = beerInsert.BrandId,
            Alcohol = beerInsert.Alcohol
        };

        await _beerRepository.Add(beer);
        await _beerRepository.Save();

        return new BeerDto
        {
            Id = beer.BeerId,
            Name = beer.Name,
            Alcohol = beer.Alcohol,
            BrandId = beer.BrandId
        };
    }

    public async Task<BeerDto> Update(int id, BeerUpdateDto beerUpdateDto)
    {
        var beer = await _beerRepository.GetById(id);

        if (beer == null)
        {
            return null;
        }

        
        beer.Name = beerUpdateDto.Name;
        beer.BrandId = beerUpdateDto.BrandId;
        beer.Alcohol = beerUpdateDto.Alcohol;

        _beerRepository.Update(beer);
        await _beerRepository.Save();

        return new BeerDto
        {
            Id = beer.BeerId,
            Name = beer.Name,
            Alcohol = beer.Alcohol,
            BrandId = beer.BrandId
        };
    }

    public async Task<BeerDto> Delete(int id)
    {
        var beer = await _beerRepository.GetById(id);

        if (beer == null)
        {
            return null;
        }

        var beerDto = new BeerDto
        {
            Id = beer.BeerId,
            Name = beer.Name,
            Alcohol = beer.Alcohol,
            BrandId = beer.BrandId
        };

        _beerRepository.Delete(beer);
        await _beerRepository.Save();

        return beerDto;
    }
}
