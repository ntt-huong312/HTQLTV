using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HTQLTV.Models;

public partial class BorrowReturn
{
    [Display(Name = "Mã mượn trả")]
    public int BorrowReturnId { get; set; }


    [Required(ErrorMessage = "Vui lòng chọn mã độc giả.")]

    [Display(Name = "Mã độc giả")]
    public int? ReaderId { get; set; }


    [Required(ErrorMessage = "Vui lòng chọn mã sách.")]

    [Display(Name = "Mã sách")]
    public int? BookId { get; set; }


    [Required(ErrorMessage = "Vui lòng nhập số sách mượn.")]
    [Display(Name = "Số sách mượn")]
    [Range(1, int.MaxValue, ErrorMessage = "Số sách mượn phải lớn hơn 0")]
    public int BookNumber { get; set; }


    [Required(ErrorMessage = "Vui lòng chọn ngày mượn.")]
    [Display(Name = "Ngày mượn")]
    [DataType(DataType.Date)] 
    public DateOnly BorrowDate { get; set; }


    [Required(ErrorMessage = "Vui lòng chọn hạn trả.")]
    [DataType(DataType.Date)]
    [Display(Name = "Hạn trả")]
    public DateOnly DueDate { get; set; }


    [Display(Name = "Ngày trả")]
    [DataType(DataType.Date)] 
    public DateOnly? ReturnDate { get; set; }


    [Required(ErrorMessage = "Vui lòng chọn mã nhân viên")]
    [Display(Name = "Mã nhân viên")]
    public int? StaffId { get; set; }

    public int? TotalBorrowed { get; set; }

    public int? TotalReturned { get; set; }

    public virtual Book? Book { get; set; }

    public virtual Reader? Reader { get; set; }

    public virtual Staff? Staff { get; set; }
}
