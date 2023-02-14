using Iot.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Domain.Events.Users
{
    public class OtpChangedEvent : EventBase
    {
        public OtpChangedEvent(Guid id, string otp, Guid? modifiedBy)
        {
            Id = id;
            Otp = otp;
            ModifiedBy = modifiedBy;
            ModifiedTime = DateTimeOffset.UtcNow;
        }
        public Guid Id { get; set; }
        public string Otp { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
    }
}
