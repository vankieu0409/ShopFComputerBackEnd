using Iot.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Profile.Domain.Events
{
    public class ProfilePhoneNumberChangedEvent:EventBase
    {
        public ProfilePhoneNumberChangedEvent(Guid id, string phoneNumber, Guid? modifiedBy)
        {
            Id = id;
            PhoneNumber = phoneNumber;
            ModifiedBy = modifiedBy;
            ModifiedTime = DateTime.UtcNow;
        }
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
    }
}
