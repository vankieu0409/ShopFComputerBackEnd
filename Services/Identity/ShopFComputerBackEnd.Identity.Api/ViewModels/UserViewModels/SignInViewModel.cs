using ShopFComputerBackEnd.Identity.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace ShopFComputerBackEnd.Identity.Api.ViewModels
{
    public class SignInViewModel
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "PassWord is required")]
        public string Password { get; set; }
        public DeviceValueObject Device{ get; set; }
    }
}
