using Iot.Core.Domain.AggregateRoots;
using Iot.Core.Extensions;
using ShopFComputerBackEnd.Product.Domain.Entities;
using ShopFComputerBackEnd.Product.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopFComputerBackEnd.Product.Domain.Aggregates
{
    public class ProductAggregateroot : FullAggregateRoot<Guid>
    {
        public ProductAggregateroot(Guid id)
        {
            if (id.IsNullOrDefault())
                throw new ArgumentNullException(nameof(id));
            Id = id;
        }
        public string Name { get; private set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string StreamName => $"Product-{Id}";
        public List<ProductVariantEntity> ProductVariants { get; private set; }
        public List<OptionEntity> Options { get; private set; }

        #region Product

        public ProductAggregateroot Initialize(string name, string description, string category, string brand, List<ProductVariantEntity> productVariants, List<OptionEntity> options, Guid createdBy)
        {
            if (productVariants.IsNullOrDefault())
                productVariants = new List<ProductVariantEntity>();
            if (options.IsNullOrDefault())
                options = new List<OptionEntity>();
            var @event = new ProductInitializedEvent(Id, name, description, category, brand, productVariants, options, createdBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }
        private void Apply(ProductInitializedEvent @event)
        {
            Id = @event.Id;
            Name = @event.Name;
            Description = @event.Description;
            Category = @event.Category;
            Brand = @event.Brand;
            ProductVariants = @event.ProductVariants;
            Options = @event.Options;
            IsDeleted = false;
            CreatedBy = @event.CreatedBy;
            CreatedTime = @event.CreatedTime;
            ModifiedBy = @event.ModefiledBy;
        }

        public ProductAggregateroot UpdateProductName(string name, Guid modefiledBy)
        {
            var @event = new ProductNameUpdatedEvent(Id, name, modefiledBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }
        private void Apply(ProductNameUpdatedEvent @event)
        {
            Name = @event.Name;
            ModifiedBy = @event.ModefiledBy;
            ModifiedTime = @event.ModefiledTime;
        }
        
        public ProductAggregateroot UpdateProductBrand(string name, Guid modefiledBy)
        {
            var @event = new ProductBrandUpdatedEvent(Id, name, modefiledBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }
        private void Apply(ProductBrandUpdatedEvent @event)
        {
            Brand = @event.Brand;
            ModifiedBy = @event.ModefiledBy;
            ModifiedTime = @event.ModefiledTime;
        }
        
        public ProductAggregateroot UpdateProductDescription(string name, Guid modefiledBy)
        {
            var @event = new ProductDescriptionUpdatedEvent(Id, name, modefiledBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }
        private void Apply(ProductDescriptionUpdatedEvent @event)
        {
            Description = @event.Description;
            ModifiedBy = @event.ModefiledBy;
            ModifiedTime = @event.ModefiledTime;
        }
        
        public ProductAggregateroot UpdateProductCategory(string name, Guid modefiledBy)
        {
            var @event = new ProductCategoryUpdatedEvent(Id, name, modefiledBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }
        private void Apply(ProductCategoryUpdatedEvent @event)
        {
            Category = @event.Category;
            ModifiedBy = @event.ModefiledBy;
            ModifiedTime = @event.ModefiledTime;
        }

        public ProductAggregateroot ProductDisable(bool isDeleted, Guid modefiledBy)
        {
            var @event = new ProductDisableUpdatedEvent(Id, isDeleted, modefiledBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }
        private void Apply(ProductDisableUpdatedEvent @event)
        {
            IsDeleted = @event.IsDeleted;
            ModifiedBy = @event.ModefiledBy;
            ModifiedTime = @event.ModefiledTime;
        }

        #endregion

        #region Option

        public ProductAggregateroot UpdateOptions(List<OptionEntity> options, Guid ModefiledBy)
        {
            var @event = new ProductOptionUpdatedEvent(Id, options, ModefiledBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }
        private void Apply(ProductOptionUpdatedEvent @event)
        {
            Options = @event.Options;
            ModifiedBy = @event.ModefiledBy;
            ModifiedTime = @event.ModefiledTime;
        }

        public ProductAggregateroot UpdateProductVariantOption(Guid productVariantId, List<OptionValueEntity> options, Guid modefiledBy)
        {
            var @event = new ProductVariantsUpdatedEvent(Id, productVariantId, options, modefiledBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }
        private void Apply(ProductVariantsUpdatedEvent @event)
        {
            ProductVariants.FirstOrDefault(entity => Guid.Equals(entity.Id, @event.ProductVariantId)).OptionValues = @event.Options;
            ModifiedBy = @event.ModefiledBy;
            ModifiedTime = @event.ModefiledTime;
        }

        public ProductAggregateroot UpdateProductVariantOptionValue(Guid productVariantOptionId, List<OptionValueEntity> optionValues, Guid modefiledBy)
        {
            var @event = new ProductOptionValueUpdatedEvent(Id, productVariantOptionId, optionValues, modefiledBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }
        private void Apply(ProductOptionValueUpdatedEvent @event)
        {
            ProductVariants.FirstOrDefault(entity => Guid.Equals(entity.Id, @event.Id)).OptionValues = @event.Values;
            ModifiedBy = @event.ModefiledBy;
            ModifiedTime = @event.ModefiledTime;
        }

        #endregion

        #region ProductVariant

        public ProductAggregateroot UpdateProductVariants(List<ProductVariantEntity> productVariants, Guid modefiledBy)
        {
            var @event = new ProductVariantUpdatedEvent(Id, productVariants, modefiledBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }
        private void Apply(ProductVariantUpdatedEvent @event)
        {

            ModifiedBy = @event.ModefiledBy;
            ModifiedTime = @event.ModefiledTime;
        }

        public ProductAggregateroot AddProductVariant(List<ProductVariantEntity> productVariants, Guid modefiledBy)
        {
            var @event = new ProductVariantAddedEvent(Id, productVariants, modefiledBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }
        private void Apply(ProductVariantAddedEvent @event)
        {
            ProductVariants.AddRange(@event.ProductVariants);
            ModifiedBy = @event.ModefiledBy;
            ModifiedTime = @event.ModefiledTime;
        }

        public ProductAggregateroot DisableProductVariant(bool isDeleted, Guid modefiledBy)
        {
            var @event = new ProductVariantDisabledEvent(Id, isDeleted, modefiledBy);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }
        private void Apply(ProductVariantDisabledEvent @event)
        {
            IsDeleted = @event.IsDisabled;
            ModifiedBy = @event.ModefiledBy;
            ModifiedTime = @event.ModefiledTime;
        }

        #endregion
    }
}
