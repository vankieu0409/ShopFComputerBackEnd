using Iot.Core.Domain.AggregateRoots;
using Iot.Core.Extensions;
using ShopFComputerBackEnd.Identity.Domain.Events.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ShopFComputerBackEnd.Identity.Domain.Enums;

namespace ShopFComputerBackEnd.Identity.Domain.Aggregates
{
    public class UserAggregateRoot : FullAggregateRoot<Guid>
    {
        public UserAggregateRoot(Guid id)
        {
            if (id.IsNullOrDefault())
                throw new ArgumentNullException("User id");
            Id = id;
        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string OtpVerify { get; set; }
        public string PasswordSalt { get; private set; }
        public string StreamName => $"User-{Id}";
        public ICollection<string> Roles { get; private set; }
        #region User

        public UserAggregateRoot Initialize(string username, string otpVerify, string password, string email, string phonenumber, Guid? createdBy)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException("User - username");
            var @event = new UserInitializedEvent(Id, otpVerify, username, password, email, phonenumber, createdBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }
        private void Apply(UserInitializedEvent @event)
        {
            Id = @event.Id;
            UserName = @event.UserName;
            OtpVerify = @event.OtpVerify;
            Roles = new Collection<string>();
            Password = @event.Password;
            Email = @event.Email;
            PhoneNumber = @event.PhoneNumber;
            CreatedBy = @event.CreatedBy;
        }
        public UserAggregateRoot ChangePassword(Guid? userId, string userName, string currentPassword, string newPassword)
        {
            var @event = new UserChangePasswordEvent(Id, userId, userName, currentPassword, newPassword);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }
        private void Apply(UserChangePasswordEvent @event)
        {
            Password = @event.NewPassword;
        }
        #endregion

        public UserAggregateRoot ForgotPassword(string newPassword)
        {
            var @event = new UserForgotPasswordEvent(Id, newPassword);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }
        private void Apply(UserForgotPasswordEvent @event)
        {
            Password = @event.NewPassword;
        }
        public UserAggregateRoot ConfirmOtp(string phoneNumber, string otp, OtpType otpType)
        {
            var @event = new OtpConfirmedEvent(Id, phoneNumber, otp, otpType);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }

        private void Apply(OtpConfirmedEvent @event)
        {
            Id = @event.Id;
            PhoneNumber = @event.PhoneNumber;
            OtpVerify = @event.Otp;
        }

        public UserAggregateRoot ChangeOtp(string otp, Guid? modifiedBy)
        {
            var @event = new OtpChangedEvent(Id, otp, modifiedBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }

        private void Apply(OtpChangedEvent @event)
        {
            Id = @event.Id;
            OtpVerify = @event.Otp;
            ModifiedBy = @event.ModifiedBy;
            ModifiedTime = @event.ModifiedTime;
        }

        public UserAggregateRoot AddToRole(ICollection<string> roleName)
        {
            if (roleName.IsNullOrDefault())
                return this;
            //if (Roles.Any(role => string.Equals(role, role)))
            //    return this;
            var @event = new UserAddedToRoleEvent(Id, roleName);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }
        private void Apply(UserAddedToRoleEvent @event)
        {
            Id = @event.Id;
            foreach (var item in @event.RoleName)
            {
                Roles.Add(item);
            }
        }
    }
}
