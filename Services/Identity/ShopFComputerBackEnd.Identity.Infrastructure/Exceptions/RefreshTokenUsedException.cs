using Iot.Core.Infrastructure.Exceptions;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Exceptions
{
    public class RefreshTokenUsedException : BusinessException
    {
        public RefreshTokenUsedException(string context, string key, int code, string message) : base(context, key, code, message)
        {
        }
    }
}
