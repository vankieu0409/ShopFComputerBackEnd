using Iot.Core.Infrastructure.Exceptions;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Exceptions
{
    public class IncorrectAccountOrPasswordException : BusinessException
    {
        public IncorrectAccountOrPasswordException(string context, string key, int code, string message) : base(context, key, code, message)
        {
        }
    }
}
