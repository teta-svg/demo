using System;
using System.Collections.Generic;

namespace demo.Context;

public partial class Order
{
    public int IdOrder { get; set; }

    public int OrderNumber { get; set; }

    public DateOnly OrderDate { get; set; }

    public int IdUser { get; set; }

    public int IdSize { get; set; }

    public int? Count { get; set; }

    public virtual Size IdSizeNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}
