using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository;

public class BrandRepository : IRepository<Brand>
{
    private StoreContext _context;

    public BrandRepository(StoreContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Brand>> Get() =>
        await _context.Brands.ToListAsync();

    public async Task<Brand> GetById(int id) =>
        await _context.Brands.FindAsync(id);

    public async Task Add(Brand entity) =>
        await _context.Brands.AddAsync(entity);

    public void Update(Brand entity)
    {
        _context.Brands.Attach(entity);
        _context.Brands.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(Brand entity) =>
        _context.Brands.Remove(entity);

    public async Task Save() =>
        await _context.SaveChangesAsync();

    public async Task<bool> ExistsByName(string name) =>
        await _context.Brands.AnyAsync(b => b.Name == name);

    public async Task<bool> ExistsByNameExcludingId(string name, int id) =>
        await _context.Brands.AnyAsync(b => b.Name == name && b.BrandId != id);

    public async Task<IEnumerable<Beer>> GetBeersByBrandId(int brandId) =>
        await _context.Beers.Where(b => b.BrandId == brandId).ToListAsync();

    public async Task<bool> HasBeers(int brandId) =>
        await _context.Beers.AnyAsync(b => b.BrandId == brandId);
}
