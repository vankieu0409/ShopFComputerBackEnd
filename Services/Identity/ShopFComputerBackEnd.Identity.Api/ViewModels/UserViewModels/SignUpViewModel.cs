using System.ComponentModel.DataAnnotations;

namespace ShopFComputerBackEnd.Identity.Api.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "DisplayName Is required")]
        public string DisplayName { get; set; }
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "PassWord is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "ConfirmPassWord is required")]
        public string ConfirmPassword { get; set; }
        public string LanguageCode { get; set; }
    }
}
