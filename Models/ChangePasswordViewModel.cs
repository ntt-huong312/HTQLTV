using System.ComponentModel.DataAnnotations;

namespace HTQLTV.Models
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Mật khẩu cũ")]
        public string CurrentPassword { get; set; }
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }
        [Display(Name = "Nhập lại mật khẩu mới")]
        public string ConfirmPassword { get; set; }
    } 
}
