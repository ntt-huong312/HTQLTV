using System;
using System.Collections.Generic;

namespace HTQLTV.Models;

public partial class BorrowReturn
{
    public int BorrowReturnId { get; set; }

    public int ReaderId { get; set; }

    public int BookId { get; set; }

    public int BookNumber { get; set; }

    public DateOnly BorrowDate { get; set; }

    public DateOnly DueDate { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public int StaffId { get; set; }

    public string StatId { get; set; } = null!;

    public virtual Book Book { get; set; } = null!;

    public virtual Reader Reader { get; set; } = null!;

    public virtual Staff Staff { get; set; } = null!;

    public virtual Statistic Stat { get; set; } = null!;
}
