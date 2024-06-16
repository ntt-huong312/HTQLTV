using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HTQLTV.Models;

public partial class Reader
{
    public int ReaderId { get; set; }

    [Required]
    [Display(Name = "Họ và tên")]
    [StringLength(50, ErrorMessage = "Họ tên không vượt quá 50 ký tự")]
    [MinLength(3, ErrorMessage = "Họ tên phải có ít nhất 3 ký tự")]
    public string FullName { get; set; } = null!;

    [Required]
    [Display(Name = "Địa chỉ")]
    [StringLength(150, ErrorMessage = "Địa chỉ không vượt quá 150 ký tự")]
    [MinLength(3, ErrorMessage = "Địa chỉ có ít nhất 3 ký tự")]
    public string ReaderAddress { get; set; } = null!;

    [Required]
    [Display(Name = "Số điện thoại")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    public string PhoneNumber { get; set; } = null!;

    [Display(Name = "Email")]
    [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; } = null!;

    [Display(Name = "Ngày sinh")]
    [Required(ErrorMessage = "Vui lòng chọn ngày sinh.")]
    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; }

    public virtual ICollection<BorrowReturn> BorrowReturns { get; set; } = new List<BorrowReturn>();
}
