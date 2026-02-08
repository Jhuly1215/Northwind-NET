using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersService.Api.Data;

namespace OrdersService.Api.Controllers.V2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/orders")]
public class OrdersController : ControllerBase
{
    private readonly OrdersDbContext _db;
    public OrdersController(OrdersDbContext db) => _db = db;

    // GET /api/v2/orders?page=1&pageSize=20&customerId=...&statusId=...
    [HttpGet]
    public async Task<ActionResult> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] int? customerId = null,
        [FromQuery] sbyte? statusId = null
    )
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var q = _db.orders.AsNoTracking();

        if (customerId.HasValue)
            q = q.Where(o => EF.Property<int?>(o, "customer_id") == customerId.Value);

        if (statusId.HasValue)
            q = q.Where(o => EF.Property<sbyte?>(o, "status_id") == statusId.Value);

        var totalCount = await q.LongCountAsync();

        var data = await (
            from o in q
            join c in _db.customers.AsNoTracking()
                on EF.Property<int?>(o, "customer_id") equals (int?)c.id into cj
            from c in cj.DefaultIfEmpty()
            orderby o.id descending
            select new
            {
                id = o.id,
                customer_id = EF.Property<int?>(o, "customer_id"),
                customer_company = c != null ? c.company : null,

                order_date = EF.Property<DateTime?>(o, "order_date"),
                status_id = EF.Property<sbyte?>(o, "status_id"),

                total =
                    _db.order_details
                        .Where(d => d.order_id == o.id)
                        .Select(d => (decimal?)(
                            ((decimal?)d.quantity ?? 0m) * ((decimal?)d.unit_price ?? 0m)
                        ))
                        .Sum() ?? 0m
            }
        )
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

        return Ok(new
        {
            page,
            pageSize,
            total = totalCount,
            items = data
        });
    }
}
