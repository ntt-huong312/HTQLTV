using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HTQLTV.Models;

public partial class Reader
{
    [Display(Name = "Mã độc giả")]
    public int ReaderId { get; set; }

    [Required(ErrorMessage = "Họ tên là bắt buộc")]
    [Display(Name = "Họ và tên")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Địa chỉ bắt buộc phải nhập.")]
    [Display(Name = "Địa chỉ")]
    public string ReaderAddress { get; set; } = null!;

    [Display(Name = "Số điện thoại")]
    [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    public string PhoneNumber { get; set; } = null!;

    [Required(ErrorMessage = "Email bắt buộc phải nhập.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Ngày sinh bắt buộc phải nhập.")]

    [Display(Name = "Ngày sinh")]
    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; }

    public virtual ICollection<BorrowReturn> BorrowReturns { get; set; } = new List<BorrowReturn>();
}
