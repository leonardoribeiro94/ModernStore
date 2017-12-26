using FluentValidator;
using ModernStore.Domain.Enums;
using ModernStore.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModernStore.Domain.Entities
{
    public class Order : Entity
    {
        protected Order() { }

        private readonly IList<OrderItem> _items;

        public Order(Customer customer, decimal deliveryFee, decimal discount)
        {
            Customer = customer;
            CreateDate = DateTime.Now;
            Number = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            Status = EOrderStatus.Created;
            DeliveryFee = deliveryFee;
            Discount = discount;
            _items = new List<OrderItem>();

            new ValidationContract<Order>(this)
                .IsGreaterThan(x => x.DeliveryFee, 0)
                .IsGreaterThan(x => x.Discount, -1);
        }

        public Customer Customer { get; set; }
        public DateTime CreateDate { get; set; }
        public string Number { get; set; }
        public EOrderStatus Status { get; set; }
        public ICollection<OrderItem> Items => _items.ToArray();
        public decimal DeliveryFee { get; set; }
        public decimal Discount { get; set; }

        public decimal SubTotal() => Items.Sum(x => x.Total());
        public decimal Total() => SubTotal() + DeliveryFee - Discount;

        public void AddItem(OrderItem orderItem)
        {
            if (orderItem.IsValid())
            {
                AddNotifications(orderItem.Notifications);
                _items.Add(orderItem);
            }

        }
    }
}
