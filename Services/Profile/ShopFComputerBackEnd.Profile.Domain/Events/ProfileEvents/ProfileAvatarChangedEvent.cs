using Iot.Core.Domain.Events;
using ShopFComputerBackEnd.Profile.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Profile.Domain.Events
{
    public class ProfileAvatarChangedEvent : EventBase
    {
        public ProfileAvatarChangedEvent(Guid id, AvatarValueObject avatar, Guid? modifiedBy)
        {
            Id = id;
            Avatar = avatar;
            ModifiedBy = modifiedBy;
            ModifiedTime = DateTime.UtcNow;
        }
        public Guid Id { get; set; }
        public AvatarValueObject Avatar { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
    }
}
