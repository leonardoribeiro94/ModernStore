using ModernStore.Domain.Commands.Results;
using ModernStore.Domain.Entities;
using System;

namespace ModernStore.Domain.Repositories
{
    public interface ICustomerRepository
    {
        Customer Get(Guid id);
        GetCustomerCommandResult Get(string userName);
        bool DocumentExists(string document);
        void Save(Customer customer);
    }
}
