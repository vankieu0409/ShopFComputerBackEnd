using Iot.Core.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Exceptions
{
    public class IncorrectPasswordFormatException : BusinessException
    {
        public IncorrectPasswordFormatException(string context, string key, int code, string message) : base(context, key, code, message)
        {
        }
    }
}
