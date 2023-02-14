using ShopFComputerBackEnd.Identity.Domain.Enums;

namespace ShopFComputerBackEnd.Identity.Api.ViewModels
{
    public class ForgotPasswordViewModel
    {
        public string PhoneNumber { get; set; }
        public string Otp { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public OtpType OtpType { get; set; }
    }
}
