using FluentValidator;
using ModernStore.Domain.Commands;
using ModernStore.Domain.Entities;
using ModernStore.Domain.Repositories;

namespace ModernStore.Domain.CommandHandler
{
    public class OrderCommandHandler : Notifiable, ICommandHandler<RegisterOrderCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderCommandHandler(ICustomerRepository customerRepository,
            IProductRepository productRepository,
            IOrderRepository orderRepository)
        {
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public void Handle(RegisterOrderCommand command)
        {

            // Instancia o cliente (Lendo do repositorio)
            var customer = _customerRepository.Get(command.Customer);

            // Gera um novo pedido
            var order = new Order(customer, command.DeliveryFee, command.Discount);

            // Adiciona os itens no pedido
            foreach (var item in command.Items)
            {
                var product = _productRepository.Get(item.Product);
                order.AddItem(new OrderItem(product, item.Quantity));
            }

            // Adiciona as notificações no pedido no Handler
            AddNotifications(order.Notifications);
            if (order.IsValid())
                _orderRepository.Save(order);
        }
    }
}
