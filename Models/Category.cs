using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HTQLTV.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    [Display(Name = "Thể loại")]
    [Required(ErrorMessage = "Nhà xuất bản bắt buộc phải nhập")]
    [StringLength(50, ErrorMessage = "Nhà xuất bản không vượt quá 50 ký tự")]
    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
