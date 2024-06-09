using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


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

    [NotMapped]
    [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Chỉ tải lên file (jpg, jpeg, png).")]
    public IFormFile? file { get; set; }
    public virtual ICollection<BorrowReturn> BorrowReturns { get; set; } = new List<BorrowReturn>();

    public virtual Category? Category { get; set; }
}
