using System;
using System.Collections.Generic;

namespace demo.Context;

public partial class Product
{
    public int IdProduct { get; set; }

    public int IdCategory { get; set; }

    public int IdSubCategory { get; set; }

    public string? Image { get; set; }

    public string ProductName { get; set; } = null!;

    public int IdProduction { get; set; }

    public string? Description { get; set; }

    public string? Composition { get; set; }

    public decimal Price { get; set; }

    public virtual Category IdCategoryNavigation { get; set; } = null!;

    public virtual Production IdProductionNavigation { get; set; } = null!;

    public virtual SubCategory IdSubCategoryNavigation { get; set; } = null!;

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual ICollection<StockItem> StockItems { get; set; } = new List<StockItem>();
}
