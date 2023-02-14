using Iot.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Profile.Domain.Events
{
    public class ProfileEmailChangedEvent:EventBase
    {
        public ProfileEmailChangedEvent(Guid id, string email, Guid? modifiedBy)
        {
            Id = id;
            Email = email;
            ModifiedBy = modifiedBy;
            ModifiedTime = DateTime.UtcNow;
        }
        public Guid Id { get; set; }
        public string Email { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
    }
}
