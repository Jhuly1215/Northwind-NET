using System;
using System.Collections.Generic;

namespace OrdersService.Api.Data.Entities;

public partial class orders_status
{
    public sbyte id { get; set; }

    public string status_name { get; set; } = null!;

    public virtual ICollection<order> orders { get; set; } = new List<order>();
}
