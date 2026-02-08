using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersService.Api.Data;

namespace OrdersService.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/shippers")]
public class ShippersController : ControllerBase
{
    private readonly OrdersDbContext _db;
    public ShippersController(OrdersDbContext db) => _db = db;

    // GET /api/v1/shippers
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var items = await _db.shippers.AsNoTracking()
            .OrderBy(s => s.id)
            .Select(s => new
            {
                id = s.id,
                company = s.company,
                first_name = s.first_name,
                last_name = s.last_name,
                business_phone = s.business_phone,
                city = s.city,
                country_region = s.country_region
            })
            .ToListAsync();

        return Ok(items);
    }
}
