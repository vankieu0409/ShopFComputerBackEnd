using Iot.Core.Domain.AggregateRoots;
using Iot.Core.Extensions;
using ShopFComputerBackEnd.Notification.Domain.Entities;
using ShopFComputerBackEnd.Notification.Domain.Enums;
using ShopFComputerBackEnd.Notification.Domain.Events;
using ShopFComputerBackEnd.Notification.Domain.Events.NotificationTemplates;
using ShopFComputerBackEnd.Notification.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShopFComputerBackEnd.Notification.Domain.Aggregates
{
    public class NotificationAggregateRoot : FullAggregateRoot<Guid>
    {
        public NotificationAggregateRoot(Guid id)
        {
            if (id.IsNullOrDefault())
                throw new ArgumentNullException("Notification id");
            Id = id;
        }
        public string StreamName => $"Notification-{Id}";

        public string Context { get; private set; }
        public string Name { get; private set; }
        public ICollection<NotificationTemplateEntity> Templates { get; private set; }
        public ICollection<NotificationVariableValueObject> Variables { get; private set; }
        public NotificationType Type { get; private set; }
        public NotificationAggregateRoot Initialize(string context, string name, NotificationTemplateEntity template, ICollection<NotificationVariableValueObject> variables, NotificationType type, Guid? createdBy)
        {
            if (string.IsNullOrEmpty(context))
                throw new ArgumentNullException("Notification context");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("Notification name");
            var @event = new NotificationInitializedEvent(Id, context, name, template, variables, type, createdBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }

        private void Apply(NotificationInitializedEvent @event)
        {
            Id = @event.Id;
            Context = @event.Context;
            Name = @event.Name;
            Templates = new Collection<NotificationTemplateEntity>();
            if (!@event.Template.IsNullOrDefault())
                Templates.Add(@event.Template);
            if (!@event.Variables.IsNullOrDefault())
                Variables = @event.Variables;
            else
                Variables = new Collection<NotificationVariableValueObject>();
            Type = @event.Type;
            CreatedBy = @event.CreatedBy;
            CreatedTime = @event.CreatedTime;
        }

        public NotificationAggregateRoot UpdateTemplate(NotificationTemplateEntity template, Guid? modifiedBy)
        {
            if (Templates.Any(temp => string.Equals(temp.LanguageCode, template.LanguageCode)))
                throw new ArgumentNullException("Notification context");
            var @event = new NotificationTemplateUpdatedEvent(Id, template, modifiedBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }

        private void Apply(NotificationTemplateUpdatedEvent @event)
        {
            Id = @event.Id;
            var template = Templates.FirstOrDefault(temp => string.Equals(temp.LanguageCode, @event.Template.LanguageCode));
            if (template.IsNullOrDefault())
                Templates.Add(@event.Template);
            else
                template = @event.Template;
            ModifiedBy = @event.ModifiedBy;
            ModifiedTime = @event.ModifiedTime;
        }

        public NotificationAggregateRoot UpdateTemplateCollection(ICollection<NotificationTemplateEntity> templates, Guid? modifiedBy)
        {
            if (templates.IsNullOrDefault())
                return this;
            var @event = new NotificationTemplateCollectionUpdatedEvent(Id, templates, modifiedBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }

        private void Apply(NotificationTemplateCollectionUpdatedEvent @event)
        {
            Id = @event.Id;
            Templates = @event.Templates;
            ModifiedBy = @event.ModifiedBy;
            ModifiedTime = @event.ModifiedTime;
        }

        public NotificationAggregateRoot UpdateVariables(ICollection<NotificationVariableValueObject> variables, Guid? modifiedBy)
        {
            if (variables.IsNullOrDefault())
                return this;
            var @event = new NotificationVariableCollectionUpdatedEvent(Id, variables, modifiedBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }

        private void Apply(NotificationVariableCollectionUpdatedEvent @event)
        {
            Id = @event.Id;
            Variables = @event.Variables;
            ModifiedBy = @event.ModifiedBy;
            ModifiedTime = @event.ModifiedTime;
        }

        public NotificationAggregateRoot SetName(string name, Guid? modifiedBy)
        {
            var @event = new NotificationNameChangedEvent(Id, name, modifiedBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }

        private void Apply(NotificationNameChangedEvent @event)
        {
            Id = @event.Id;
            Name = @event.Name;
            ModifiedBy = @event.ModifiedBy;
            ModifiedTime = @event.ModifiedTime;
        }

        public NotificationAggregateRoot SetType(NotificationType type, Guid? modifiedBy)
        {
            var @event = new NotificationTypeChangedEvent(Id, type, modifiedBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }

        private void Apply(NotificationTypeChangedEvent @event)
        {
            Id = @event.Id;
            Type = @event.Type;
            ModifiedBy = @event.ModifiedBy;
            ModifiedTime = @event.ModifiedTime;
        }

        public NotificationAggregateRoot Delete(Guid? deletedBy)
        {
            var @event = new NotificationDeletedEvent(Id, deletedBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }

        private void Apply(NotificationDeletedEvent @event)
        {
            Id = @event.Id;
            IsDeleted = true;
            DeletedBy = @event.DeletedBy;
            DeletedTime = @event.DeletedTime;
        }

        public NotificationAggregateRoot Recover(Guid? modifiedBy)
        {
            var @event = new NotificationRecoveredEvent(Id, modifiedBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }

        private void Apply(NotificationRecoveredEvent @event)
        {
            Id = @event.Id;
            IsDeleted = false;
            ModifiedBy = @event.ModifiedBy;
            ModifiedTime = @event.ModifiedTime;
        }

        public NotificationAggregateRoot RemoveTemplate(string languageCode, Guid? modifiedBy)
        {
            if (string.IsNullOrEmpty(languageCode))
                return this;
            var templateToRemove = Templates.FirstOrDefault(template => string.Equals(template.LanguageCode, languageCode));
            if (templateToRemove.IsNullOrDefault())
                return this;
            var @event = new NotificationTemplateRemovedEvent(Id, templateToRemove, modifiedBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }

        private void Apply(NotificationTemplateRemovedEvent @event)
        {
            Id = @event.Id;
            Templates.Remove(@event.Template);
            ModifiedBy = @event.ModifiedBy;
            ModifiedTime = @event.ModifiedTime;
        }
    }
}
