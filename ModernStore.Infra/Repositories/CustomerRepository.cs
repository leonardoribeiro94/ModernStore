using Dapper;
using ModernStore.Domain.Commands.Results;
using ModernStore.Domain.Entities;
using ModernStore.Domain.Repositories;
using ModernStore.Infra.DataContexts;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace ModernStore.Infra.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ModernStoreDataContext _context;

        public CustomerRepository(ModernStoreDataContext context)
        {
            _context = context;
        }

        public Customer Get(Guid id)
        {
            return _context
                .Customers
                .Include(x => x.User)
                .FirstOrDefault(x => x.Id == id);
        }

        public GetCustomerCommandResult Get(string userName)
        {
            #region Retorno consulta usando EF

            /*
            return _context
                .Customers
                .Include(x => x.User)
                .AsNoTracking()
                .Select(x => new CustomerCommandResult
                {
                    Name = x.Name.ToString(),
                    Document = x.Document.Number,
                    Active = x.User.Active,
                    Email = x.Email.Address,
                    Password = x.User.PassWord,
                    UserName = x.User.UserName
                })
                .FirstOrDefault(x => x.UserName == userName);
                */

            #endregion

            using (var conn = new SqlConnection(@""))
            {
                conn.Open();
                const string query = @"SELECT * FROM GetLoginInfoView 
                              WHERE [Active] = 1 AND [UserName] = @userName";
                var parameter = new { userName };

                return conn.Query<GetCustomerCommandResult>(query, parameter).FirstOrDefault();
            }
        }

        public bool DocumentExists(string document)
        {
            return _context
                .Customers
                .Any(x => x.Document.Number.Contains(document));
        }

        public void Update(Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Save(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }
    }
}
