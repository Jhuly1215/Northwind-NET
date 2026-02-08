using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersService.Api.Contracts;
using OrdersService.Api.Data;

namespace OrdersService.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/orders")]
public class OrdersController : ControllerBase
{
    private readonly OrdersDbContext _db;

    public OrdersController(OrdersDbContext db) => _db = db;

    // Helpers para convertir int/int? -> sbyte/sbyte?
    private static sbyte ToSByte(int v) => checked((sbyte)v);
    private static sbyte? ToSByteNullable(int? v) => v.HasValue ? checked((sbyte)v.Value) : (sbyte?)null;

    // GET /api/v1/orders?page=1&pageSize=20&customerId=...&statusId=...
    [HttpGet]
    public async Task<ActionResult<PagedResult<OrderListItemDto>>> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] int? customerId = null,
        [FromQuery] sbyte? statusId = null
    )
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var q = _db.orders.AsNoTracking();

        if (customerId.HasValue) q = q.Where(o => o.customer_id == customerId.Value);
        if (statusId.HasValue) q = q.Where(o => o.status_id == statusId.Value);

        var total = await q.LongCountAsync();

        var items = await q
            .OrderByDescending(o => o.id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(o => new OrderListItemDto(
                o.id,
                o.customer_id,
                o.order_date,
                o.status_id
            ))
            .ToListAsync();

        return Ok(new PagedResult<OrderListItemDto>(items, page, pageSize, total));
    }

    // GET /api/v1/orders/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDetailsDto>> GetById(int id)
    {
        var o = await _db.orders.AsNoTracking().FirstOrDefaultAsync(x => x.id == id);
        if (o is null) return NotFound();

        var items = await (
            from od in _db.order_details.AsNoTracking()
            join p in _db.products.AsNoTracking() on od.product_id equals p.id into pj
            from p in pj.DefaultIfEmpty()
            where od.order_id == id
            orderby od.id
            select new OrderItemDto(
                od.id,
                od.product_id,
                p != null ? p.product_name : null,
                od.quantity,
                od.unit_price,
                od.status_id
            )
        ).ToListAsync();

        return Ok(new OrderDetailsDto(
            o.id,
            o.customer_id,
            o.employee_id,
            o.order_date,
            o.shipped_date,
            o.shipper_id,
            o.ship_name,
            o.ship_city,
            o.shipping_fee,
            o.taxes,
            o.status_id,
            items
        ));
    }

    // POST /api/v1/orders
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateOrderRequest req)
    {
        if (req.items is null || req.items.Count == 0)
            return BadRequest("items is required");

        await using var tx = await _db.Database.BeginTransactionAsync();

        var o = new OrdersService.Api.Data.Entities.order
        {
            customer_id = req.customer_id,
            employee_id = req.employee_id,
            shipper_id = req.shipper_id,
            ship_name = req.ship_name,
            ship_city = req.ship_city,
            order_date = DateTime.UtcNow,

            // status_id en DB es sbyte? -> convertir desde int?/int
            status_id = req.status_id.HasValue ? ToSByte(req.status_id.Value) : (sbyte?)0
        };

        _db.orders.Add(o);
        await _db.SaveChangesAsync(); // genera o.id

        foreach (var it in req.items)
        {
            var od = new OrdersService.Api.Data.Entities.order_detail
            {
                order_id = o.id,
                product_id = it.product_id,
                quantity = it.quantity ?? 1,
                unit_price = it.unit_price ?? 0,

                // status_id en DB es sbyte? -> convertir desde int?
                status_id = ToSByteNullable(it.status_id)
            };
            _db.order_details.Add(od);
        }

        await _db.SaveChangesAsync();
        await tx.CommitAsync();

        return CreatedAtAction(nameof(GetById), new { version = "1.0", id = o.id }, new { id = o.id });
    }

    // PUT /api/v1/orders/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateOrderRequest req)
    {
        var o = await _db.orders.FirstOrDefaultAsync(x => x.id == id);
        if (o is null) return NotFound();

        await using var tx = await _db.Database.BeginTransactionAsync();

        o.customer_id = req.customer_id;
        o.employee_id = req.employee_id;
        o.shipper_id = req.shipper_id;
        o.ship_name = req.ship_name;
        o.ship_city = req.ship_city;

        // si viene status_id, lo casteas; si no viene, lo dejas como está
        if (req.status_id.HasValue)
            o.status_id = ToSByte(req.status_id.Value);

        // estrategia simple: reemplazar items
        var existing = await _db.order_details.Where(d => d.order_id == id).ToListAsync();
        _db.order_details.RemoveRange(existing);

        if (req.items != null)
        {
            foreach (var it in req.items)
            {
                _db.order_details.Add(new OrdersService.Api.Data.Entities.order_detail
                {
                    order_id = id,
                    product_id = it.product_id,
                    quantity = it.quantity ?? 1,
                    unit_price = it.unit_price ?? 0,
                    status_id = ToSByteNullable(it.status_id)
                });
            }
        }

        await _db.SaveChangesAsync();
        await tx.CommitAsync();
        return NoContent();
    }

    // PATCH /api/v1/orders/{id}/status
    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult> PatchStatus(int id, [FromBody] PatchOrderStatusRequest req)
    {
        var o = await _db.orders.FirstOrDefaultAsync(x => x.id == id);
        if (o is null) return NotFound();

        // req.status_id probablemente es int
        o.status_id = ToSByte(req.status_id);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/v1/orders/{id}
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await using var tx = await _db.Database.BeginTransactionAsync();

        var o = await _db.orders.FirstOrDefaultAsync(x => x.id == id);
        if (o is null) return NotFound();

        var details = await _db.order_details.Where(d => d.order_id == id).ToListAsync();
        _db.order_details.RemoveRange(details);
        _db.orders.Remove(o);

        await _db.SaveChangesAsync();
        await tx.CommitAsync();
        return NoContent();
    }
}
