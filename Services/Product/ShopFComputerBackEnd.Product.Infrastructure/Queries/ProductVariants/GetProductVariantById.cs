using MediatR;
using ShopFComputerBackEnd.Product.Domain.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Product.Infrastructure.Queries.ProductVariants
{
    public class GetProductVariantById : IRequest<ProductVariantDto>
    {
        public GetProductVariantById(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
