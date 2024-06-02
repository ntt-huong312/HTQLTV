using System;
using System.Collections.Generic;

namespace HTQLTV.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string FullName { get; set; } = null!;

    public string Position { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<BorrowReturn> BorrowReturns { get; set; } = new List<BorrowReturn>();
}
