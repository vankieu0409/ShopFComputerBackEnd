using Iot.Core.Infrastructure.Exceptions;

namespace ShopFComputerBackEnd.Product.Infrastructure.Exceptions.Products
{
    public class PriceNotValidateException : BusinessException
    {
        public PriceNotValidateException(string priceName,string message) : base($"{priceName} is not validate","Error Value Price",301,message)
        {
        }
    }
}
