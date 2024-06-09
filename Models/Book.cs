using System;
using System.Collections.Generic;

namespace HTQLTV.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string Publisher { get; set; } = null!;

    public int YearPublished { get; set; }

    public int? CategoryId { get; set; }

    public int Quantity { get; set; }

    public int Available { get; set; }

    public string? BookImage { get; set; }

    public virtual ICollection<BorrowReturn> BorrowReturns { get; set; } = new List<BorrowReturn>();

    public virtual Category? Category { get; set; }
}
