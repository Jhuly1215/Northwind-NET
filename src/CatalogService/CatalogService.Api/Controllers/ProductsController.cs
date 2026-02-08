using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CatalogService.Api.Data;
using CatalogService.Api.Data.Entities;

namespace CatalogService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly NorthwindDbContext _db;

    public ProductsController(NorthwindDbContext db) => _db = db;

    // GET /api/products?skip=0&take=20
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll([FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        take = Math.Clamp(take, 1, 200);

        var items = await _db.Products
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return Ok(items);
    }

    // GET /api/products/10
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var item = await _db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return item is null ? NotFound() : Ok(item);
    }

    // POST /api/products
    [HttpPost]
    public async Task<ActionResult<Product>> Create(Product input)
    {
        _db.Products.Add(input);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = input.Id }, input);
    }

    // PUT /api/products/10
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Product input)
    {
        if (id != input.Id) return BadRequest("Id mismatch.");

        var exists = await _db.Products.AnyAsync(x => x.Id == id);
        if (!exists) return NotFound();

        _db.Entry(input).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/products/10
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Products.FindAsync(id);
        if (item is null) return NotFound();

        _db.Products.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
