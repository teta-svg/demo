using System;
using System.Collections.Generic;

namespace demo.Context;

public partial class SubCategory
{
    public int IdSubCategory { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
