using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Users
{
    public class VerifyOtpQuery : IRequest<bool>
    {
        public VerifyOtpQuery(string user, string otp)
        {
            User = user;
            Otp = otp;
        }

        public string User { get; set; }
        public string Otp { get; set; }
    }
}
