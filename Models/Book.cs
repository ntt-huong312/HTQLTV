using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
namespace HTQLTV.Models;

public partial class Book
{
    public int BookId { get; set; }

    [Display(Name = "Tên sách")]
    [Required]
    public string Title { get; set; } = null!;

    [Display(Name = "Tác giả")]
    [Required]
    public string Author { get; set; } = null!;

    [Display(Name = "Nhà xuất bản")]
    [Required]
    public string Publisher { get; set; } = null!;

    [Display(Name = "Năm xuất bản")]
    [Required]
    public int YearPublished { get; set; }

    [Display(Name = "Thể loại")]
    [Required]
    public int CategoryId { get; set; }

    [Display(Name = "Số lượng")]
    [Required]
    public int Quantity { get; set; }

    [Display(Name = "Có sẵn")]
    public int Available { get; set; }

    [Display(Name = "Hình ảnh")]
    [Required]
    public string BookImage { get; set; } = null!;

    [NotMapped]
    [Display(Name = "Tệp hình ảnh")]
    public IFormFile? file { get; set; }

    public virtual ICollection<BorrowReturn> BorrowReturns { get; set; } = new List<BorrowReturn>();

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Statistic> Statistics { get; set; } = new List<Statistic>();

    
}
