using System;
using System.Collections.Generic;

namespace OrdersService.Api.Data.Entities;

public partial class product
{
    public string? supplier_ids { get; set; }

    public int id { get; set; }

    public string? product_code { get; set; }

    public string? product_name { get; set; }

    public string? description { get; set; }

    public decimal? standard_cost { get; set; }

    public decimal list_price { get; set; }

    public int? reorder_level { get; set; }

    public int? target_level { get; set; }

    public string? quantity_per_unit { get; set; }

    public bool discontinued { get; set; }

    public int? minimum_reorder_quantity { get; set; }

    public string? category { get; set; }

    public byte[]? attachments { get; set; }

    public virtual ICollection<order_detail> order_details { get; set; } = new List<order_detail>();
}
