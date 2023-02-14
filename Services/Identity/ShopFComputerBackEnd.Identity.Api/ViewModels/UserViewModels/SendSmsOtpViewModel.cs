using ShopFComputerBackEnd.Identity.Domain.Enums;

namespace ShopFComputerBackEnd.Identity.Api.ViewModels
{
    public class SendSmsOtpViewModel
    {
        public string PhoneNumber { get; set; }
        public string LanguageCode { get; set; }
        public TypeOtp CaseOtp { get; set; }
    }
}
