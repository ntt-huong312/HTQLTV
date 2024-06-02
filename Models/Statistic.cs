using System;
using System.Collections.Generic;

namespace HTQLTV.Models;

public partial class Statistic
{
    public string StatId { get; set; } = null!;

    public int BookId { get; set; }

    public int TotalBorrowed { get; set; }

    public int TotalReturned { get; set; }

    public int CurrentBorrowed { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual ICollection<BorrowReturn> BorrowReturns { get; set; } = new List<BorrowReturn>();
}
