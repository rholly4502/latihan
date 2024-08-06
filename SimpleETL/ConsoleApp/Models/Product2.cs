using System;
using System.Collections.Generic;

namespace SimpleETL.Models;

public partial class Product2
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Nama { get; set; } = null!;

    public int Stock { get; set; }

    public decimal Price { get; set; }
}
