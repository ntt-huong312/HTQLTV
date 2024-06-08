using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HTQLTV.Models;

public partial class Staff
{
    [Display(Name = "Mã nhân viên")]
    public int StaffId { get; set; }

    [Required(ErrorMessage = "Họ tên là bắt buộc")]
    [Display(Name = "Họ và tên")]
    [StringLength(50, ErrorMessage = "Họ tên không vượt quá 50 ký tự")]
    [MinLength(3, ErrorMessage = "Họ tên phải có ít nhất 3 ký tự")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Vị trí là bắt buộc")]
    [Display(Name = "Vị trí")]
    [StringLength(20, ErrorMessage = "Vị trí không vượt quá 20 ký tự")]
    [MinLength(3, ErrorMessage = "Vị trí phải có ít nhất 3 ký tự")]
    public string Position { get; set; } = null!;

    [Display(Name = "Số điện thoại")]
    [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    public string PhoneNumber { get; set; } = null!;

    [Required(ErrorMessage = "Email là bắt buộc")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    public virtual ICollection<BorrowReturn> BorrowReturns { get; set; } = new List<BorrowReturn>();
}
