using System;
using System.Collections.Generic;
using ShopFComputerBackEnd.Order.Domain.Entities;

namespace ShopFComputerBackEnd.Order.Api.ViewModels;

public abstract class OrderViewModelBase
{
    public Guid? ProfileId { get; set; }
    public long AmountPay { get; set; }
    public long PayingCustomer { get; set; }
    public string Description { get; set; }
    public long Payments { get; set; }
    public int StatusOrder { get; set; }
    public bool IsEnabled { get; set; }
    public ICollection<OrderDetailViewModel> OrderDetailCollection { get; set; }
}