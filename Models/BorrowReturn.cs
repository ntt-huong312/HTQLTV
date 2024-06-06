using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HTQLTV.Models;

public partial class BorrowReturn
{
    [Display(Name = "Mã mượn trả")]
    public int BorrowReturnId { get; set; }
    [Required]    
    
    [Display(Name = "Mã độc giả")]
    public int ReaderId { get; set; }

    [Required]
    [Display(Name = "Mã sách")]
    public int BookId { get; set; }

   [Required]
    [Display(Name = "Số sách mượn")]
    public int BookNumber { get; set; }

    [Required]
    [Display(Name = "Ngày mượn")]
    public DateOnly BorrowDate { get; set; }

    [Required]
    [Display(Name = "Hạn trả")]
    public DateOnly DueDate { get; set; }

    [Display(Name = "Ngày trả")]
    public DateOnly? ReturnDate { get; set; }

    [Required]
    [Display(Name = "Mã nhân viên")]
    public int StaffId { get; set; }

  
    [Display(Name = "Số sách mượn")]
    public int? TotalBorrowed { get; set; }

   
    [Display(Name = "Số sách trả")]
    public int? TotalReturned { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Reader Reader { get; set; } = null!;

    public virtual Staff Staff { get; set; } = null!;
}
