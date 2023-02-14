using System;
using System.ComponentModel.DataAnnotations;

namespace ShopFComputerBackEnd.Identity.Api.ViewModels
{
    public class ChangePasswordViewModelBase
    {
        [Required(ErrorMessage = " CurrentPassword is required")]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "New PassWord is required")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Confirm PassWord is required")]
        public string ConfirmPassword { get; set; }
    }
}
