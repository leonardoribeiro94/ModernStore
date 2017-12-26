using FluentValidator;
using ModernStore.Domain.Commands.Inputs;
using ModernStore.Domain.Commands.Results;
using ModernStore.Domain.Entities;
using ModernStore.Domain.Repositories;
using ModernStore.Domain.Resources;
using ModernStore.Domain.Services;
using ModernStore.Domain.ValueObjects;
using ModernStore.Shared.Commands;

namespace ModernStore.Domain.Commands.Handler
{
    public class CustomerCommandHandler : Notifiable,
        ICommandHandler<RegisterCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmailService _emailService;

        public CustomerCommandHandler(ICustomerRepository customerRepository, IEmailService emailService)
        {
            _customerRepository = customerRepository;
            _emailService = emailService;
        }

        public ICommandResult Handle(RegisterCustomerCommand command)
        {
            //Passo 1  Recupera o cloente
            if (_customerRepository.DocumentExists(command.Document))
            {
                AddNotification("Document", "Este CPF já está em uso!");
                return null;
            }


            //passo 2. Gerar novo cliente
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document);
            var email = new Email(command.Email);
            var user = new User(command.UserName, command.LastName, command.ConfirmPassword);

            var customer = new Customer(name, email, document, user);

            //passo 3. Adicionar as notificações
            AddNotifications(name.Notifications);
            AddNotifications(document.Notifications);
            AddNotifications(email.Notifications);
            AddNotifications(user.Notifications);
            AddNotifications(customer.Notifications);


            //Passo 4. Inserir no banco
            if (IsValid())
                _customerRepository.Save(customer);


            //passo 5. Enviar E-mail de boas vindas
            _emailService.Send(customer.Name.ToString(),
                customer.Email.Address,
                string.Format(EmailTemplates.WelcomeEmailTitle, customer.Name),
                string.Format(EmailTemplates.WelcomeEmailBody, customer.Name));


            //passo 6. Retornar algo
            return new RegisterCustomerCommandResult(customer.Id, customer.Name.ToString());
        }
    }
}
