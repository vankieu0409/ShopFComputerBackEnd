using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Domain.Dtos
{
    public class OtpDto
    {
        public Guid UserId { get; set; }
        public string OtpVerify { get; set; }
    }
}
