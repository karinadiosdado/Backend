using backend.DTOs;
using backend.Models;
using backend.Repository;

namespace backend.services;

public class BrandService : ICommonServices<BrandDto, BrandInsertDto, BrandUpdateDto>
{
    private readonly IRepository<Brand> _brandRepository;
    private readonly IRepository<Beer> _beerRepository;

    public BrandService(
        IRepository<Brand> brandRepository,
        IRepository<Beer> beerRepository)
    {
        _brandRepository = brandRepository;
        _beerRepository = beerRepository;
    }

    public async Task<IEnumerable<BrandDto>> Get()
    {
        var brands = await _brandRepository.Get();
        return brands.Select(b => new BrandDto
        {
            Id = b.BrandId,
            Name = b.Name,
        });
    }

    public async Task<BrandDto> GetById(int id)
    {
        var brand = await _brandRepository.GetById(id);

        if (brand == null)
        {
            return null;
        }

        return new BrandDto
        {
            Id = brand.BrandId,
            Name = brand.Name,
        };
    }

    public async Task<BrandDto?> Add(BrandInsertDto brandInsert)
    {
        // Impedir nombre duplicado
        if (_brandRepository is BrandRepository repo)
        {
            if (await repo.ExistsByName(brandInsert.Name))
            {
                return null;
            }
        }
        else
        {
            var brands = await _brandRepository.Get();
            if (brands.Any(b => string.Equals(b.Name, brandInsert.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return null;
            }
        }

        var brand = new Brand
        {
            Name = brandInsert.Name
        };

        await _brandRepository.Add(brand);
        await _brandRepository.Save();

        return new BrandDto
        {
            Id = brand.BrandId,
            Name = brand.Name,
        };
    }

    public async Task<BrandDto?> Update(int id, BrandUpdateDto brandUpdateDto)
    {
        var brand = await _brandRepository.GetById(id);

        if (brand == null)
        {
            return null;
        }

        // Impedir nombre duplicado al actualizar (excluyendo el actual)
        if (_brandRepository is BrandRepository repo)
        {
            if (await repo.ExistsByNameExcludingId(brandUpdateDto.Name, id))
            {
                return null;
            }
        }
        else
        {
            var brands = await _brandRepository.Get();
            if (brands.Any(b => b.BrandId != id && string.Equals(b.Name, brandUpdateDto.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return null;
            }
        }

        brand.Name = brandUpdateDto.Name;

        _brandRepository.Update(brand);
        await _brandRepository.Save();

        return new BrandDto
        {
            Id = brand.BrandId,
            Name = brand.Name,
        };
    }

    public async Task<BrandDto?> Delete(int id)
    {
        var brand = await _brandRepository.GetById(id);

        if (brand == null)
        {
            return null;
        }

        var brandDto = new BrandDto
        {
            Id = brand.BrandId,
            Name = brand.Name,
        };

        _brandRepository.Delete(brand);
        await _brandRepository.Save();

        return brandDto;
    }

    //  Obtener cervezas asociadas a la marca
    public async Task<IEnumerable<BeerDto>?> GetBeersByBrandId(int brandId)
    {
        var brand = await _brandRepository.GetById(brandId);
        if (brand == null)
        {
            return null;
        }

        if (_brandRepository is BrandRepository repo)
        {
            var beers = await repo.GetBeersByBrandId(brandId);
            return beers.Select(b => new BeerDto
            {
                Id = b.BeerId,
                Name = b.Name,
                Alcohol = b.Alcohol,
                BrandId = b.BrandId
            });
        }
        else
        {
            var beers = await _beerRepository.Get();
            return beers
                .Where(b => b.BrandId == brandId)
                .Select(b => new BeerDto
                {
                    Id = b.BeerId,
                    Name = b.Name,
                    Alcohol = b.Alcohol,
                    BrandId = b.BrandId
                });
        }
    }

    // Impedir eliminar una marca con cervezas asociadas
    public async Task<bool> HasBeers(int brandId)
    {
        if (_brandRepository is BrandRepository repo)
        {
            return await repo.HasBeers(brandId);
        }

        var beers = await _beerRepository.Get();
        return beers.Any(b => b.BrandId == brandId);
    }
}
