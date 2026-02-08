using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersService.Api.Data;

namespace OrdersService.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/customers")]
public class CustomersController : ControllerBase
{
    private readonly OrdersDbContext _db;
    public CustomersController(OrdersDbContext db) => _db = db;

    // GET /api/v1/customers?q=alfreds&page=1&pageSize=20
    [HttpGet]
    public async Task<ActionResult> Get([FromQuery] string? q = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = _db.customers.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.Trim();
            query = query.Where(c =>
                (c.company != null && c.company.Contains(q)) ||
                (c.first_name != null && c.first_name.Contains(q)) ||
                (c.last_name != null && c.last_name.Contains(q)));
        }

        var total = await query.LongCountAsync();

        var items = await query
            .OrderBy(c => c.id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new
            {
                id = c.id,
                company = c.company,
                first_name = c.first_name,
                last_name = c.last_name,
                city = c.city,
                country_region = c.country_region
            })
            .ToListAsync();

        return Ok(new { page, pageSize, total, items });
    }

    // GET /api/v1/customers/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        var c = await _db.customers.AsNoTracking().FirstOrDefaultAsync(x => x.id == id);
        if (c is null) return NotFound();

        return Ok(new
        {
            id = c.id,
            company = c.company,
            first_name = c.first_name,
            last_name = c.last_name,
            email = c.email_address,
            city = c.city,
            state_province = c.state_province,
            country_region = c.country_region,
            business_phone = c.business_phone
        });
    }

    // GET /api/v1/customers/{id}/orders
    [HttpGet("{id:int}/orders")]
    public async Task<ActionResult> GetOrders(int id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var q = _db.orders.AsNoTracking().Where(o => o.customer_id == id);

        var total = await q.LongCountAsync();

        var items = await q.OrderByDescending(o => o.id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(o => new
            {
                id = o.id,
                order_date = o.order_date,
                status_id = o.status_id,
                shipped_date = o.shipped_date
            })
            .ToListAsync();

        return Ok(new { page, pageSize, total, items });
    }
}
