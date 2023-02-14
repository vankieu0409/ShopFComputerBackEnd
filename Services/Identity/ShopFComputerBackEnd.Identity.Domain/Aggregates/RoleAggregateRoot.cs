using Iot.Core.Domain.AggregateRoots;
using Iot.Core.Extensions;
using ShopFComputerBackEnd.Identity.Domain.Events.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Domain.Aggregates
{
    public class RoleAggregateRoot:FullAggregateRoot<Guid>
    {
        public RoleAggregateRoot(Guid id)
        {
            if (id.IsNullOrDefault())
                throw new ArgumentNullException("Role Id!");
            Id = id;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string StreamName => $"Role-{Id}";

        public RoleAggregateRoot Initialize(string name, Guid? createdBy)
        {
            if (name.IsNullOrDefault())
                throw new ArgumentNullException("name");
            var @event = new RoleInitializedEvent(Id, name, createdBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }

        private void Apply(RoleInitializedEvent @event)
        {
            Id = @event.Id;
            Name = @event.Name;
        }

        public RoleAggregateRoot SetName(string name, Guid? modifiedBy)
        {
            if (name.IsNullOrDefault() || string.Equals(Name, name))
                return this;
            var @event = new RoleNameChangedEvent(Id, name, modifiedBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }

        private void Apply(RoleNameChangedEvent @event)
        {
            Id = @event.Id;
            Name = @event.Name;
            ModifiedBy = @event.ModifiedBy;
            ModifiedTime = @event.ModifiedTime;
        }


        public RoleAggregateRoot Delete(Guid? deletedBy)
        {
            var @event = new RoleDeletedEvent(Id, deletedBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }

        private void Apply(RoleDeletedEvent @event)
        {
            Id = @event.Id;
            DeletedBy = @event.DeletedBy;
            IsDeleted = @event.IsDeleted;
            DeletedTime = @event.DeletedTime;
        }

        public RoleAggregateRoot Recover(Guid? modifiedBy)
        {
            var @event = new RoleRecoveredEvent(Id, modifiedBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }

        private void Apply(RoleRecoveredEvent @event)
        {
            Id = @event.Id;
            IsDeleted = @event.IsDeleted;
            ModifiedBy = @event.ModifiedBy;
            ModifiedTime = @event.ModifiedTime;
        }
    }
}
