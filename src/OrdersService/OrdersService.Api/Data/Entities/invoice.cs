using System;
using System.Collections.Generic;

namespace OrdersService.Api.Data.Entities;

public partial class invoice
{
    public int id { get; set; }

    public int? order_id { get; set; }

    public DateTime? invoice_date { get; set; }

    public DateTime? due_date { get; set; }

    public decimal? tax { get; set; }

    public decimal? shipping { get; set; }

    public decimal? amount_due { get; set; }

    public virtual order? order { get; set; }
}
