using Iot.Core.Infrastructure.Exceptions;

namespace ShopFComputerBackEnd.Profile.Api.Exceptions
{
    public class ForbidException : BusinessException
    {
        public ForbidException(string message) : base("Profile", "Forbidden", 403, message)
        {
        }
    }
}
