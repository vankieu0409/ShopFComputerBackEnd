using ShopFComputerBackEnd.Identity.Domain.Enums;

namespace ShopFComputerBackEnd.Identity.Api.ViewModels
{
    public class ConfirmOtpViewModel
    {

        public string PhoneNumber { get; set; }
        public string Otp { get; set; }
        public OtpType OtpType { get; set; }
    }
}
