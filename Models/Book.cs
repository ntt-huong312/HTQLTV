using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTQLTV.Models;

public partial class Book
{
    public int BookId { get; set; }

    [Display(Name ="Tên sách")]
    public string Title { get; set; } = null!;
    [Display(Name = "Tác giả")]
    public string Author { get; set; } = null!;

    [Display(Name = "Nhà xuất bản")]
    public string Publisher { get; set; } = null!;

    [Display(Name = "Năm xuất bản")]
    public int YearPublished { get; set; }

    [Display(Name = "Thể loại")]
    public int CategoryId { get; set; }

    [Display(Name = "Số lượng")]
    public int Quantity { get; set; }

    public int Available { get; set; }

    public string ?BookImage { get; set; }

    [NotMapped]
    public IFormFile? file { get; set; }
    public virtual ICollection<BorrowReturn> BorrowReturns { get; set; } = new List<BorrowReturn>();

    public virtual Category Category { get; set; } = null!;
}
