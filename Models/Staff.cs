using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HTQLTV.Models;

public partial class Staff
{
    [Display(Name = "Mã nhân viên")]
    public int StaffId { get; set; }

    [Required]
    [Display(Name = "Họ và tên")]
    public string FullName { get; set; } = null!;

    [Required]
    [Display(Name = "Vị trí")]
    public string Position { get; set; } = null!;

    [Required]
    [Display(Name = "Số điện thoại")]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    public virtual ICollection<BorrowReturn> BorrowReturns { get; set; } = new List<BorrowReturn>();
}
