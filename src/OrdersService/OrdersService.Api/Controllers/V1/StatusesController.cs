using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersService.Api.Data;

namespace OrdersService.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/orders")]
public class StatusesController : ControllerBase
{
    private readonly OrdersDbContext _db;
    public StatusesController(OrdersDbContext db) => _db = db;

    // GET /api/v1/orders/statuses
    [HttpGet("statuses")]
    public async Task<ActionResult> GetOrderStatuses()
        => Ok(await _db.orders_statuses.AsNoTracking()
            .OrderBy(x => x.id)
            .Select(x => new { id = x.id, name = x.status_name })
            .ToListAsync());

    // GET /api/v1/orders/tax-statuses
    [HttpGet("tax-statuses")]
    public async Task<ActionResult> GetTaxStatuses()
        => Ok(await _db.orders_tax_statuses.AsNoTracking()
            .OrderBy(x => x.id)
            .Select(x => new { id = x.id, name = x.tax_status_name })
            .ToListAsync());

    // GET /api/v1/orders/detail-statuses
    [HttpGet("detail-statuses")]
    public async Task<ActionResult> GetDetailStatuses()
        => Ok(await _db.order_details_statuses.AsNoTracking()
            .OrderBy(x => x.id)
            .Select(x => new { id = x.id, name = x.status_name })
            .ToListAsync());
}
