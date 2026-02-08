using System;
using System.Collections.Generic;

namespace OrdersService.Api.Data.Entities;

public partial class order
{
    public int id { get; set; }

    public int? employee_id { get; set; }

    public int? customer_id { get; set; }

    public DateTime? order_date { get; set; }

    public DateTime? shipped_date { get; set; }

    public int? shipper_id { get; set; }

    public string? ship_name { get; set; }

    public string? ship_address { get; set; }

    public string? ship_city { get; set; }

    public string? ship_state_province { get; set; }

    public string? ship_zip_postal_code { get; set; }

    public string? ship_country_region { get; set; }

    public decimal? shipping_fee { get; set; }

    public decimal? taxes { get; set; }

    public string? payment_type { get; set; }

    public DateTime? paid_date { get; set; }

    public string? notes { get; set; }

    public double? tax_rate { get; set; }

    public sbyte? tax_status_id { get; set; }

    public sbyte? status_id { get; set; }

    public virtual customer? customer { get; set; }

    public virtual employee? employee { get; set; }

    public virtual ICollection<invoice> invoices { get; set; } = new List<invoice>();

    public virtual ICollection<order_detail> order_details { get; set; } = new List<order_detail>();

    public virtual shipper? shipper { get; set; }

    public virtual orders_status? status { get; set; }

    public virtual orders_tax_status? tax_status { get; set; }
}
