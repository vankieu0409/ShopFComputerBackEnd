using Iot.Core.Domain.Events;

using System;

namespace ShopFComputerBackEnd.Profile.Domain.Events
{
    public class ProfileNicknameChangedEvent : EventBase
    {
        public ProfileNicknameChangedEvent(Guid id, string nickname, Guid? modifiedBy)
        {
            Id = id;
            Nickname = nickname;
            ModifiedBy = modifiedBy;
            ModifiedTime = DateTimeOffset.UtcNow;
        }
        public Guid Id { get; set; }
        public string Nickname { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
    }
}
