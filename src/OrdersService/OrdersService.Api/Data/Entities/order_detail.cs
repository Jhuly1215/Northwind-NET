using System;
using System.Collections.Generic;

namespace OrdersService.Api.Data.Entities;

public partial class order_detail
{
    public int id { get; set; }

    public int order_id { get; set; }

    public int? product_id { get; set; }

    public decimal quantity { get; set; }

    public decimal? unit_price { get; set; }

    public double discount { get; set; }

    public int? status_id { get; set; }

    public DateTime? date_allocated { get; set; }

    public int? purchase_order_id { get; set; }

    public int? inventory_id { get; set; }

    public virtual order order { get; set; } = null!;

    public virtual product? product { get; set; }

    public virtual order_details_status? status { get; set; }
}
