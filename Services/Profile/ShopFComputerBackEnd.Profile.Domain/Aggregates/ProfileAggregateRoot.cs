using Iot.Core.Domain.AggregateRoots;
using Iot.Core.Extensions;
using ShopFComputerBackEnd.Profile.Domain.Events;
using ShopFComputerBackEnd.Profile.Domain.ValueObjects;
using System;
using ShopFComputerBackEnd.Profile.Domain.Enums;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Profile.Domain.Aggregates
{
    public class ProfileAggregateRoot : FullAggregateRoot<Guid>
    {
        public ProfileAggregateRoot(Guid id)
        {
            if (id.IsNullOrDefault())
                throw new ArgumentNullException("Profile Id");
            Id = id;
        }
        public string StreamName => $"Profile-{Id}";
        public Guid UserId { get; private set; }
        public string DisplayName { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public GendersType Gender { get; private set; }
        public AvatarValueObject Avatar { get; private set; }
        public List<AddressValueObject> Address { get; private set; }

        public ProfileAggregateRoot Initialize(Guid userId, string displayName, string email, string phoneNumber, GendersType gender, AvatarValueObject avatar, List<AddressValueObject> address, Guid? createdBy)
        {
            var @event = new ProfileInitializedEvent(Id, userId, displayName, email, phoneNumber, gender, avatar, address, createdBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }

        public void Apply(ProfileInitializedEvent @event)
        {
            Id = @event.Id;
            UserId = @event.UserId;
            DisplayName = @event.DisplayName;
            Email = @event.Email;
            PhoneNumber = @event.PhoneNumber;
            Gender = @event.Gender;
            Avatar = @event.Avatar;
            Address = @event.Address;
            CreatedBy = @event.CreatedBy;
            ModifiedBy = @event.CreatedBy;
            CreatedTime = @event.CreatedTime;
        }

        public ProfileAggregateRoot SetDisplayName(string displayName, Guid? modifiedBy)
        {
            var @event = new ProfileDisplayNameChangedEvent(Id, displayName, modifiedBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }
        public void Apply(ProfileDisplayNameChangedEvent @event)
        {
            Id = @event.Id;
            DisplayName = @event.DisplayName;
            ModifiedBy = @event.ModifiedBy;
            ModifiedTime = @event.ModifiedTime;
        }

        public ProfileAggregateRoot SetEmail(string email, Guid? modifiedBy)
        {
            var @event = new ProfileEmailChangedEvent(Id, email, modifiedBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }
        public void Apply(ProfileEmailChangedEvent @event)
        {
            Id = @event.Id;
            Email = @event.Email;
            ModifiedBy = @event.ModifiedBy;
            ModifiedTime = @event.ModifiedTime;
        }

        public ProfileAggregateRoot SetPhoneNumber(string phoneNumber, Guid? modifiedBy)
        {
            var @event = new ProfilePhoneNumberChangedEvent(Id, phoneNumber, modifiedBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }
        public void Apply(ProfilePhoneNumberChangedEvent @event)
        {
            Id = @event.Id;
            PhoneNumber = @event.PhoneNumber;
            ModifiedBy = @event.ModifiedBy;
            ModifiedTime = @event.ModifiedTime;
        }

        public ProfileAggregateRoot SetGender(GendersType gender, Guid? modifiedBy)
        {
            var @event = new ProfileGenderChangedEvent(Id, gender, modifiedBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }
        public void Apply(ProfileGenderChangedEvent @event)
        {
            Id = @event.Id;
            Gender = @event.Gender;
            ModifiedBy = @event.ModifiedBy;
            ModifiedTime = @event.ModifiedTime;
        }

        public ProfileAggregateRoot SetAvatar(AvatarValueObject avatar, Guid? modifiedBy)
        {
            var @event = new ProfileAvatarChangedEvent(Id, avatar, modifiedBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }
        public void Apply(ProfileAvatarChangedEvent @event)
        {
            Id = @event.Id;
            Avatar = @event.Avatar;
            ModifiedBy = @event.ModifiedBy;
            ModifiedTime = @event.ModifiedTime;
        }

        public ProfileAggregateRoot SetAddress(List<AddressValueObject> address, Guid? modifiedBy)
        {
            var @event = new ProfileAddressChangedEvent(Id, address, modifiedBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }
        public void Apply(ProfileAddressChangedEvent @event)
        {
            Id = @event.Id;
            Address = @event.Address;
            ModifiedBy = @event.ModifiedBy;
            ModifiedTime = @event.ModifiedTime;
        }

        public ProfileAggregateRoot Delete(Guid? deletedBy)
        {
            var @event = new ProfileDeletedEvent(Id, deletedBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }
        public void Apply(ProfileDeletedEvent @event)
        {
            Id = @event.Id;
            IsDeleted = true;
            DeletedBy = @event.DeletedBy;
            DeletedTime = @event.DeletedTime;
        }
    }
}
