using System.ComponentModel.DataAnnotations;

namespace HTQLTV.Models
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "Email")]
        [Required]
        public string Email { get; set; }
        [Display(Name = "Mật khẩu cũ")]
        [Required]
        public string CurrentPassword { get; set; }
        [Display(Name = "Mật khẩu mới")]
  
        [Required]
        public string NewPassword { get; set; } 
        [Display(Name = "Nhập lại mật khẩu mới")]
        [Required]
        public string ConfirmPassword { get; set; }
    } 
}
/*Required(ErrorMessage = "Số điện thoại là bắt buộc")]*/