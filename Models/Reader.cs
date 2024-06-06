using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HTQLTV.Models;

public partial class Reader
{
    [Display(Name = "Mã độc giả")]
    public int ReaderId { get; set; }

    [Required]
    [Display(Name = "Họ và tên")]
    public string FullName { get; set; } = null!;

    [Required]
    [Display(Name = "Địa chỉ")]
    public string ReaderAddress { get; set; } = null!;

    [Required]
    [Display(Name = "Số điện thoại")]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    [Required]
    [Display(Name = "Ngày sinh")]
    public DateOnly DateOfBirth { get; set; }

    public virtual ICollection<BorrowReturn> BorrowReturns { get; set; } = new List<BorrowReturn>();
}
