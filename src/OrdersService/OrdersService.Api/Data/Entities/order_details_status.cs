using System;
using System.Collections.Generic;

namespace OrdersService.Api.Data.Entities;

public partial class order_details_status
{
    public int id { get; set; }

    public string status_name { get; set; } = null!;

    public virtual ICollection<order_detail> order_details { get; set; } = new List<order_detail>();
}
