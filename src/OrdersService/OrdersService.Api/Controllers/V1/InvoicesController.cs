using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersService.Api.Data;

namespace OrdersService.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/orders/{orderId:int}/invoices")]
public class InvoicesController : ControllerBase
{
    private readonly OrdersDbContext _db;
    public InvoicesController(OrdersDbContext db) => _db = db;

    // GET /api/v1/orders/{orderId}/invoices
    [HttpGet]
    public async Task<ActionResult> Get(int orderId)
    {
        var exists = await _db.orders.AsNoTracking().AnyAsync(o => o.id == orderId);
        if (!exists) return NotFound("order not found");

        var items = await _db.invoices.AsNoTracking()
            .Where(i => i.order_id == orderId)
            .OrderByDescending(i => i.invoice_date)
            .Select(i => new
            {
                id = i.id,
                order_id = i.order_id,
                invoice_date = i.invoice_date,
                due_date = i.due_date,
                tax = i.tax,
                shipping = i.shipping,
                amount_due = i.amount_due
            })
            .ToListAsync();

        return Ok(items);
    }
}
