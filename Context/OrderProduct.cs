using System;
using System.Collections.Generic;

namespace demo.Context;

public partial class OrderProduct
{
    public int IdOrderProduct { get; set; }

    public int IdOrder { get; set; }

    public int IdProduct { get; set; }

    public virtual Order IdOrderNavigation { get; set; } = null!;

    public virtual Product IdProductNavigation { get; set; } = null!;
}
