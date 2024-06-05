using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTQLTV.Models;

public partial class BorrowReturn
{
    [Key]
    public int BorrowReturnId { get; set; }

    public int ReaderId { get; set; }

    public int BookId { get; set; }

    public int BookNumber { get; set; }

    public DateOnly BorrowDate { get; set; }

    public DateOnly DueDate { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public int StaffId { get; set; }

    public string StatId { get; set; } = null!;

    [ForeignKey("BookId")]
    public virtual Book Book { get; set; } = null!;
    [ForeignKey("ReaderId")]
    public virtual Reader Reader { get; set; } = null!;
    [ForeignKey("StaffId")]
    public virtual Staff Staff { get; set; } = null!;

    [ForeignKey("StatId")]
    public virtual Statistic Stat { get; set; } = null!;
}
