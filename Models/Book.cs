using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HTQLTV.Models;

public partial class Book
{
    public int BookId { get; set; }

    [Display(Name = "Tên sách")]
    [Required(ErrorMessage = "Tiêu đề tên sách bắt buộc nhập.")]
    [StringLength(150, ErrorMessage = "Tên sách không vượt quá 150 ký tự")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Tên tác giả bắt buộc phải nhập")]
    [StringLength(50, ErrorMessage = "Tên tác giả không vượt quá 50 ký tự")]
    public string Author { get; set; } = null!;

    [Display(Name = "Nhà xuất bản")]
    [Required(ErrorMessage = "Nhà xuất bản bắt buộc phải nhập")]
    [StringLength(50, ErrorMessage = "Nhà xuất bản không vượt quá 100 ký tự")]
    public string Publisher { get; set; } = null!;

    [Display(Name = "Năm xuất bản")]
    [Required(ErrorMessage = "Năm xuất bản bắt buộc phải nhập.")]
    [Range(1900, 2100, ErrorMessage = "Năm xuất bản phải nằm trong khoảng 1900 đến 2100.")]
    public int YearPublished { get; set; }

    [Display(Name = "Thể loại")]
    [Required(ErrorMessage = "Vui lòng chọn thể loại sách.")]
    public int? CategoryId { get; set; }

    [Display(Name = "Số lượng")]
    [Required(ErrorMessage = "Số lượng sách bắt buộc phải nhập.")]
    [Range(0, int.MaxValue, ErrorMessage = "Số lượng sách phải là số dương")]
    public int Quantity { get; set; }

    [Display(Name = "Có sẵn")]
    public int Available { get; set; }

    [Display(Name = "Hình ảnh")]
    public string? BookImage { get; set; }

    [NotMapped]
    //[FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Chỉ tải lên file (jpg, jpeg, png).")]
    public IFormFile? file { get; set; }
    public virtual ICollection<BorrowReturn> BorrowReturns { get; set; } = new List<BorrowReturn>();

    public virtual Category? Category { get; set; }
}
