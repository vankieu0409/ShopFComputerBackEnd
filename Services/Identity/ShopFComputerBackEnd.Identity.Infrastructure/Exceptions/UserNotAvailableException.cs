using Iot.Core.Infrastructure.Exceptions;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Exceptions
{
    public class UserNotAvailableException : BusinessException
    {
        public UserNotAvailableException(string context, string key, int code, string message) : base(context, key, code, message)
        {
        }
    }
}
