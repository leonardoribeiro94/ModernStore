using ModernStore.Domain.CommandHandler;
using ModernStore.Domain.Commands;
using ModernStore.Domain.Entities;
using ModernStore.Domain.Repositories;
using ModernStore.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace ModernStore.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var command = new RegisterOrderCommand
            {
                Customer = Guid.NewGuid(),
                DeliveryFee = 9,
                Discount = 30,
                Items = new List<RegisterOrderItemCommand>
                {
                    new RegisterOrderItemCommand()
                    {
                        Product = Guid.NewGuid(),
                        Quantity = 3
                    }
                }
            };

            GenerateOrder(new FakeCustomerRepository(),
                new FakeProductRepository(),
                new FakeOrderRepository(),
                command);
            Console.ReadKey();
        }

        private static void GenerateOrder(
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            RegisterOrderCommand command)
        {

            var handler = new OrderCommandHandler(customerRepository, productRepository, orderRepository);
            handler.Handle(command);

            if (handler.IsValid())
                Console.WriteLine("Registro cadastrado com Sucesso!");
        }
    }


    public class FakeCustomerRepository : ICustomerRepository
    {
        public Customer Get(Guid id)
        {
            return null;
        }

        public Customer GetByUserId(Guid id)
        {
            var name = new Name("Leonardo", "Ribeiro");
            var email = new Email("leonardostudent94@gmail.com");
            var document = new Document("42432131860");
            var user = new User("leonardo", "1234");
            var dateBirth = new DateTime(1994, 06, 26);

            return new Customer(name, dateBirth, email, document, user);
        }
    }

    public class FakeProductRepository : IProductRepository
    {
        public Product Get(Guid id)
        {
            return new Product("mouse", 299, "", 50);
        }
    }

    public class FakeOrderRepository : IOrderRepository
    {
        public void Save(Order order)
        {

        }
    }
}
