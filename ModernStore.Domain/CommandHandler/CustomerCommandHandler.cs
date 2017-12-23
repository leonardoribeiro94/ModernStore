using FluentValidator;
using ModernStore.Domain.Commands;
using ModernStore.Domain.Repositories;
using ModernStore.Domain.ValueObjects;

namespace ModernStore.Domain.CommandHandler
{
    public class CustomerCommandHandler : Notifiable
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public void Handle(UpdateCustomerCommand command)
        {
            //Passo 1  Recupera o cloente
            var customer = _customerRepository.Get(command.Id);

            //passo 2. Caso o cliente nao exista
            if (customer == null)
            {
                AddNotification("customer", "Cliente não encontrado");
                return;
            }

            //passo3. Atualizar a entidade
            var name = new Name(command.FirstName, command.LastName);
            customer.Update(name, command.BirthDate);

            //passo 4. Adiciona as notificações
            AddNotifications(customer.Notifications);

            //passo 5. Persistir no banco
            if (customer.IsValid())
                _customerRepository.Update(customer);
        }
    }
}
