using System;
using System.Collections.Generic;

namespace demo.Context;

public partial class Production
{
    public int IdProduction { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<StockItem> StockItems { get; set; } = new List<StockItem>();
}
