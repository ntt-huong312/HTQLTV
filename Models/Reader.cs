using System;
using System.Collections.Generic;

namespace HTQLTV.Models;

public partial class Reader
{
    public int ReaderId { get; set; }

    public string FullName { get; set; } = null!;

    public string ReaderAddress { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public virtual ICollection<BorrowReturn> BorrowReturns { get; set; } = new List<BorrowReturn>();
}
