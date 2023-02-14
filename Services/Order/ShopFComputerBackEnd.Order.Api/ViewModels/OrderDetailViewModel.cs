using System;

namespace ShopFComputerBackEnd.Order.Api.ViewModels;

public class OrderDetailViewModel
{
    public Guid ProductVariantId { get; set; }
    public int Quantity { get; set; }
    public Int64 UnitPrice { get; set; }
    public bool IsEnabled { get; set; }
}